
import request from '@/utils/request'

export function GetComplainData() {
  return request({
    url: '/api/Complain/GetComplainData',
    method: 'get'
  })
}

export function InsertComplainData(data) {
  return request({
    url: '/api/Complain/InsertComplainData',
    method: 'post',
    data
  })
}

export function GetUpdateData(data) {
  return request({
    url: '/api/Complain/GetUpdateData',
    method: 'post',
    data
  })
}

export function UpdateComplainData(data) {
  return request({
    url: '/api/Complain/UpdateComplainData',
    method: 'put',
    data
  })
}
