<template>
  <div class="app-container">
    <div class="filter-container" >
      <el-input v-model="listQuery.key" placeholder="Title" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter" />
      <el-select v-model="listQuery.importance" placeholder="Imp" clearable style="width: 90px" class="filter-item">
        <el-option v-for="item in importanceOptions" :key="item" :label="item" :value="item" />
      </el-select>
      <el-select v-model="listQuery.type" placeholder="Type" clearable class="filter-item" style="width: 130px">
        <el-option v-for="item in calendarTypeOptions" :key="item.key" :label="item.display_name+'('+item.key+')'" :value="item.key" />
      </el-select>
      <el-select v-model="listQuery.sort" style="width: 140px" class="filter-item" @change="handleFilter">
        <el-option v-for="item in sortOptions" :key="item.key" :label="item.label" :value="item.key" />
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">
        Search
      </el-button>
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">
        Add
      </el-button>
      <el-checkbox v-model="showReviewer" class="filter-item" style="margin-left:15px;" @change="tableKey=tableKey+1">
        reviewer
      </el-checkbox>
    </div>

    <el-table :key="tableKey" v-loading="listLoading" :data="list" border fit highlight-current-row style="width: 100%;">
      <el-table-column label="序号11" type="index" align="center" width="70" />
      <el-table-column label="下单时间" align="center" width="150">
        <template slot-scope="{row}">
          <span>{{ row.createdTime | parseTime('{y}-{m}-{d} {h}:{i}') }}</span>
        </template>
      </el-table-column>
      <el-table-column label="产品" prop="content" align="center" min-width="150" />
      <el-table-column label="订单类型" align="center" width="150">
        <template slot-scope="{row}">
          <el-tag>{{ row.orderType | orderTypeFilter }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="支付方式" align="center" width="150">
        <template slot-scope="{row}">
          <el-tag>{{ row.payMethod | payMethodFilter }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="订单状态" align="center" width="150">
        <template slot-scope="{row}">
          <el-tag>{{ row.orderState | orderStateFilter }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="金额" prop="amount" align="center" width="150" />
    </el-table>
  </div>
</template>

<script>
import { getPaymentOrders } from '@/api/payment-order'
import waves from '@/directive/waves'
import { debuglog } from 'util';

export default {
  name: 'ComplexTable',
  directives: { waves },
  filters: {
    orderTypeFilter(type) {
      const orderTypeMap = {
        0: '充值',
        1: '消费'
      }
      return orderTypeMap[type] || ''
    },
    payMethodFilter(method) {
      const payMethodMap = {
        0: '支付宝',
        1: '微信',
        2: '余额'
      }
      return payMethodMap[method] || ''
    },
    orderStateFilter(state) {
      const orderStateMap = {
        0: '等待支付',
        1: '已支付',
        2: '支付失败',
        3: '已核销'
      }
      return orderStateMap[state] || ''
    }
  },
  data() {
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        key: undefined,
        page: 1,
        limit: 20
      },
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.listLoading = true
      getPaymentOrders(this.listQuery).then(res => {
        this.list = res.data.data
        this.total = res.data.total
        this.listLoading = false
      })
    },
    handleDetail(row) {
    }
  }
}
</script>
<style scoped>
  .el-table,.el-button,.el-tag{
    font-size: 16px!important;
  }
</style>

