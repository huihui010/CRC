import request from '@/utils/request'

export function GetSignatureData() {
  return request({
    url: '/api/ClientContract/GetSignatureData',
    method: 'get'
  })
}

export function InsertSignatureData(data) {
  return request({
    url: '/api/ClientContract/InsertSignatureData',
    method: 'post',
    data
  })
}

