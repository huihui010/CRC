import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/api/Login/CheckLog',
    method: 'post',
    data
  })
}
// 获取用户详情
export function getUserInfo(data) {
  return request({
    url: '/api/Login/ReturnData',
    method: 'post',
    data
  })
}
// 获取路由
export function getMoveRoutr(params) {
  return request({
    url: '/api/Login/CheckLog',
    method: 'get',
    params
  })
}
