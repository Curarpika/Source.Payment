import request from '@/utils/request'

export function getPaymentOrders(data) {
  return request({
    url: '/Home/GetPaymentOrders',
    method: 'get',
    params: data
  })
}

export function getPaidOrders() {
  return request({
    url: '/Home/GetPaidOrders',
    method: 'get'
  })
}

export function processOrder(data) {
  return request({
    url: `/Home/ProcessOrder?orderId=${data}`,
    method: 'post'
  })
}

export function dashboard(data) {
  return request({
    url: `/Home/Dashboard?dateStart=${data.dateStart}&dateEnd=${data.dateEnd}`,
    method: 'post'
  })
}

