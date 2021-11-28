import request from '@/utils/request'

export function GetPlanData() {
  return request({
    url: '/api/Action/GetPlanData',
    method: 'get'
  })
}

export function InsertPlanData(data) {
  return request({
    url: '/api/Action/InsertPlanData',
    method: 'post',
    data
  })
}

export function UpdatePlanData(data) {
  return request({
    url: '/api/Action/UpdatePlanData',
    method: 'put',
    data
  })
}

export function RemovePlanData(data) {
  return request({
    url: '/api/Action/RemovePlanData',
    method: 'delete',
    data
  })
}

export function GetUpdateData(data) {
  return request({
    url: '/api/Action/GetUpdateData',
    method: 'post',
    data
  })
}

export function GetActionData() {
  return request({
    url: '/api/Action/GetActionData',
    method: 'get'
  })
}

export function InsertActionData(data) {
  return request({
    url: '/api/Action/InsertActionData',
    method: 'post',
    data
  })
}

