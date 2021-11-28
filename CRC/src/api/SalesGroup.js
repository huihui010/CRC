import request from '@/utils/request'

export function GetGroupData() {
  return request({
    url: '/api/SalesmanGroup/GetGroupData',
    method: 'get'
  })
}

export function InsertGroupData(data) {
  return request({
    url: '/api/SalesmanGroup/InsertGroupData',
    method: 'post',
    data
  })
}

export function GetUpdateData(data) {
  return request({
    url: '/api/SalesmanGroup/GetUpdateData',
    method: 'post',
    data
  })
}

export function UpdateGroupData(data) {
  return request({
    url: '/api/SalesmanGroup/UpdateGroupData',
    method: 'put',
    data
  })
}

export function RemoveGroupData(data) {
  return request({
    url: '/api/SalesmanGroup/RemoveGroupData',
    method: 'delete',
    data
  })
}
