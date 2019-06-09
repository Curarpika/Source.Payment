export const orderTypeOptions = [
  { key: 0, label: '积分充值' },
  { key: 1, label: '线上消费' },
  { key: 2, label: '线下消费' }
]

export const orderTypeKeyValue = orderTypeOptions.reduce((acc, cur) => {
  acc[cur.key] = cur.label
  return acc
}, {})

export const payMethodOptions = [
  { key: 0, label: '支付宝' },
  { key: 1, label: '微信' },
  { key: 2, label: '余额' }
]

export const payMethodKeyValue = payMethodOptions.reduce((acc, cur) => {
  acc[cur.key] = cur.label
  return acc
}, {})

export const orderStateOptions = [
  { key: 0, label: '等待支付' },
  { key: 1, label: '已支付' },
  { key: 2, label: '支付失败' }
]

export const orderStateKeyValue = orderStateOptions.reduce((acc, cur) => {
  acc[cur.key] = cur.label
  return acc
}, {})

export const productOrderStateOptions = [
  { key: 0, label: '已下单' },
  { key: 1, label: '执行中' },
  { key: 2, label: '订单完成' }
]

export const productOrderStateKeyValue = productOrderStateOptions.reduce((acc, cur) => {
  acc[cur.key] = cur.label
  return acc
}, {})

