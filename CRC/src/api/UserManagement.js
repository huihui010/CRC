import request from '@/utils/request'

export function GetUsersData() {
  return request({
    url: '/api/Users/GetUsersData',
    method: 'get'
  })
}

export function InsertUsersData(data) {
  return request({
    url: '/api/Users/InsertUsersData',
    method: 'post',
    data
  })
}

export function UpdateUsersData(data) {
  return request({
    url: '/api/Users/UpdateUsersData',
    method: 'put',
    data
  })
}

export function RemoveUsersData(data) {
  return request({
    url: '/api/Users/RemoveUsersData',
    method: 'delete',
    data
  })
}

export function GetUpdateData(data) {
  return request({
    url: '/api/Users/GetUpdateData',
    method: 'post',
    data
  })
}

export function GetPersonData() {
  return request({
    url: '/api/Users/GetPersonData',
    method: 'get'
  })
}

