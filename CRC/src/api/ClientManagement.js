import request from '@/utils/request'

export function GetClientData() {
  return request({
    url: '/api/ClientInfo/GetClientData',
    method: 'get'
  })
}

export function InsertClientData(data) {
  return request({
    url: '/api/ClientInfo/InsertClientdata',
    method: 'post',
    data
  })
}

export function UpdateClientData(data) {
  return request({
    url: '/api/ClientInfo/UpdateClientData',
    method: 'put',
    data
  })
}

export function RemoveClientData(data) {
  return request({
    url: '/api/ClientInfo/RemoveClientData',
    method: 'delete',
    data
  })
}

export function GetUpdateData(data) {
  return request({
    url: '/api/ClientInfo/GetUpdateData',
    method: 'post',
    data
  })
}

