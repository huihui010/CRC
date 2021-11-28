import request from '@/utils/request'

export function GetSalesmanData() {
  return request({
    url: '/api/Salesman/GetSalesmanData',
    method: 'get'
  })
}

export function InsertSalesman(data) {
  return request({
    url: '/api/Salesman/InsertSalesmanData',
    method: 'post',
    data
  })
}

export function UpdateSalesman(data) {
  return request({
    url: '/api/Salesman/UpdateSalesmanData',
    method: 'put',
    data
  })
}

export function RemoveSalesman(data) {
  return request({
    url: '/api/Salesman/RemoveSalesmanData',
    method: 'delete',
    data
  })
}

export function GetUpdateData(data) {
  return request({
    url: '/api/Salesman/GetUpdateData',
    method: 'post',
    data
  })
}

