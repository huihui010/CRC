import request from '@/utils/request'

export function GetLinkmanData() {
  return request({
    url: '/api/ClientLinkman/GetLinkmanData',
    method: 'get'
  })
}

export function InsertLinkmanData(data) {
  return request({
    url: '/api/ClientLinkman/InsertLinkmanData',
    method: 'post',
    data
  })
}

export function UpdateLinkmanData(data) {
  return request({
    url: '/api/ClientLinkman/UpdateLinkmanData',
    method: 'put',
    data
  })
}

export function RemoveLinkmanData(data) {
  return request({
    url: '/api/ClientLinkman/RemoveLinkmanData',
    method: 'delete',
    data
  })
}

export function GetUpdateData(data) {
  return request({
    url: '/api/ClientLinkman/GetUpdateData',
    method: 'post',
    data
  })
}

