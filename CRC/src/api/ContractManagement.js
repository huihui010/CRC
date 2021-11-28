import request from '@/utils/request'

export function GetContractData() {
  return request({
    url: '/api/ClientContract/GetContractData',
    method: 'get'
  })
}

export function InsertContractData(data) {
  return request({
    url: '/api/ClientContract/InsertContractData',
    method: 'post',
    data
  })
}

export function UpdateContractDataData(data) {
  return request({
    url: '/api/ClientContract/UpdateContractData',
    method: 'put',
    data
  })
}

export function RemoveContractDataData(data) {
  return request({
    url: '/api/ClientContract/RemoveContractData',
    method: 'delete',
    data
  })
}

export function GetUpdateData(data) {
  return request({
    url: '/api/ClientContract/GetUpdateData',
    method: 'post',
    data
  })
}

