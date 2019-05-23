import request from '@/utils/request'

export function getPaidOrders() {
  return request({
    url: '/user/GetPaidOrders',
    method: 'get'
  })
}
