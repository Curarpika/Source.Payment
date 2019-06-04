export const orderTypeOptions = [
  { key: 0, label: '充值' },
  { key: 1, label: '消费' }
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
  { key: 2, label: '支付失败' },
  { key: 3, label: '已核销' }
]

export const orderStateKeyValue = orderStateOptions.reduce((acc, cur) => {
  acc[cur.key] = cur.label
  return acc
}, {})

