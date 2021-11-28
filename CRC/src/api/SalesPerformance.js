import request from '@/utils/request'

export function GetPerformanceData() {
  return request({
    url: '/api/SalesmanPerformance/GetPerformanceData',
    method: 'get'
  })
}

export function InsertPerformanceData(data) {
  return request({
    url: '/api/SalesmanPerformance/InsertPerformanceData',
    method: 'post',
    data
  })
}

export function UpdatePerformanceData(data) {
  return request({
    url: '/api/SalesmanPerformance/UpdatePerformanceData',
    method: 'put',
    data
  })
}

export function RemovePerformanceData(data) {
  return request({
    url: '/api/SalesmanPerformance/RemovePerformanceData',
    method: 'delete',
    data
  })
}

export function GetUpdateData(data) {
  return request({
    url: '/api/SalesmanPerformance/GetUpdateData',
    method: 'post',
    data
  })
}

