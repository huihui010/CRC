// import { logout } from '@/api/user'
import { login, getUserInfo } from '@/api/index'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { resetRouter } from '@/router'

const getDefaultState = () => {
  return {
    token: getToken(),
    name: '',
    avatar: '',
    roles: [],
    button: []
  }
}

const state = getDefaultState()

const mutations = {
  RESET_STATE: (state) => {
    Object.assign(state, getDefaultState())
  },
  SET_TOKEN: (state, token) => {
    state.token = token
  },
  SET_NAME: (state, name) => {
    state.name = name
  },
  // 设置roles
  SET_ROLES: (state, roles) => {
    state.roles = roles
  },
  SET_BUTTON: (state, button) => {
    state.button = button
  }
}

const actions = {
  // user login
  login({ commit }, userInfo) {
    const { username, password } = userInfo
    return new Promise((resolve, reject) => {
      login({ userName: username.trim(), userPassword: password }).then(response => {
        // const { data } = response

        if (response.code !== 200) {
          return this.$message.error('账号或密码错误！请重新登录')
        } else {
          commit('SET_TOKEN', response.token)
          setToken(response.token)
          resolve()
        }
      }).catch(error => {
        reject('账号或密码错误, 请输入！【状态：' + error + '】')
      })
    })
  },

  // get user info
  getInfo({ commit, state }) {
    return new Promise((resolve, reject) => {
      getUserInfo(state.token).then(response => {
        const { code } = response
        // console.log('token校验：' + response.msg)
        if (code !== 200) {
          return reject('检验失败, 请重新登录.')
        }
        const { userIDCard } = response
        commit('SET_NAME', userIDCard)
        const { rolePermission } = response
        // console.log('权限是：' + rolePermission)
        commit('SET_ROLES', rolePermission)
        commit('SET_BUTTON', rolePermission)
        // commit('SET_AVATAR', avatar)
        resolve(response)
      }).catch(error => {
        reject('检验失败, 请重新登录！【状态：' + error + '】')
      })
    })
  },

  // user logout
  logout({ commit, state }) {
    return new Promise((resolve, reject) => {
      // logout(state.token).then(() => {
      removeToken() // must remove  token  first
      resetRouter()
      commit('RESET_STATE')
      resolve()
      // }).catch(error => {
      //   reject(error)
      // })
    })
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {
      removeToken() // must remove  token  first
      commit('RESET_STATE')
      commit('SET_ROLES', [])
      resolve()
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}

