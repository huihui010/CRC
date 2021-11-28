import { asyncRouterMap, constantRoutes } from '@/router/index'

function hasPermission(roles, route) {
  if (route.meta && route.meta.roles) {
    return roles.some(role =>
      route.meta.roles.includes(role)
    )
  } else {
    return true
  }
}

/**
 * Filter asynchronous routing tables by recursion
 * @param routes asyncRoutes
 * @param roles
 */
export function filterAsyncRoutes(routes, roles) {
  const res = []
  routes.forEach(route => {
    const tmp = { ...route }
    if (hasPermission(roles, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRoutes(tmp.children, roles)
      }
      res.push(tmp)
    }
  })
  return res
}

const state = {
  routes: [],
  addRoutes: []
}

const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  }
}

const actions = {
  generateRoutes({ commit }, roles) {
    return new Promise(resolve => {
      let accessedRoutes
      if (roles.includes(4)) {
        // 路由是否有admin,有直接全部显示
        accessedRoutes = asyncRouterMap || []
      } else {
        // accessedRoutes这个就是当前角色可见的动态路由
        accessedRoutes = filterAsyncRoutes(asyncRouterMap, roles)
      }
      commit('SET_ROUTES', accessedRoutes)
      resolve(accessedRoutes)
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
