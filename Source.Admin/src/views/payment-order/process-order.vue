<template>
  <div class="app-container">
    <div class="filter-container" />

    <el-table :key="tableKey" v-loading="listLoading" :data="list" border fit highlight-current-row style="width: 100%;">
      <el-table-column label="序号" type="index" align="center" width="70" />
      <el-table-column label="支付时间" align="center" width="150">
        <template slot-scope="{row}">
          <span>{{ row.paidTime | parseTime('{y}-{m}-{d} {h}:{i}') }}</span>
        </template>
      </el-table-column>
      <el-table-column label="产品" prop="content" align="center" min-width="150" />
      <!-- <el-table-column label="订单类型" align="center" width="150">
        <template slot-scope="{row}">
          <el-tag>{{ row.orderType | orderTypeFilter }}</el-tag>
        </template>
      </el-table-column> -->
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
      <el-table-column label="核销码" prop="code" align="center" min-width="150" />
      <el-table-column label="操作" align="center" width="200" class-name="small-padding fixed-width">
        <template slot-scope="{row}">
          <el-button v-show="row.orderState === 1 || row.orderState === 3" :disabled="row.orderState === 3" type="primary" size="mini" @click="handleProcessOrder(row)">
            核销
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import { getPaidOrders, processOrder } from '@/api/payment-order'
import waves from '@/directive/waves'

export default {
  name: 'ComplexTable',
  directives: { waves },
  mqtt: {
    'PaidOrders': function(val) {
        this.listLoading = true
        var str = this.uintToString(val)
        var newOrder = JSON.parse(str)
        this.list.unshift(newOrder)
        this.listLoading = false
    }
  },
  filters: {
    // orderTypeFilter(type) {
    //   const orderTypeMap = {
    //     0: '充值',
    //     1: '消费'
    //   }
    //   return orderTypeMap[type] || ''
    // },
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
      listLoading: true
    }
  },
  created() {
    this.getList()
  },
  mounted () {
    this.$mqtt.subscribe('PaidOrders')
  },
  methods: {
    getList() {
      this.listLoading = true
      getPaidOrders().then(res => {
        this.list = res.data
        this.total = res.data.length
        this.listLoading = false
      })
    },
    handleProcessOrder(row) {
      processOrder(row.id).then(res => {
        if (res) {
          this.$message({
            message: '核销成功',
            type: 'success'
          })

          this.listLoading = true

          setTimeout(() => {
            const index = this.list.indexOf(row)
            this.list.splice(index, 1)
            this.listLoading = false
          }, 1 * 1000)

          row.orderState = 3
         }
      })
    },
    uintToString(uintArray) {
      var encodedString = String.fromCharCode.apply(null, uintArray),
          decodedString = decodeURIComponent(escape(encodedString));
      return decodedString;
    },
    utf8ByteToUnicodeStr(utf8Bytes){
      var unicodeStr ="";
      for (var pos = 0; pos < utf8Bytes.length;){
          var flag= utf8Bytes[pos];
          var unicode = 0 ;
          if ((flag >>>7) === 0 ) {
              unicodeStr+= String.fromCharCode(utf8Bytes[pos]);
              pos += 1;

          } else if ((flag &0xFC) === 0xFC ){
              unicode = (utf8Bytes[pos] & 0x3) << 30;
              unicode |= (utf8Bytes[pos+1] & 0x3F) << 24;
              unicode |= (utf8Bytes[pos+2] & 0x3F) << 18;
              unicode |= (utf8Bytes[pos+3] & 0x3F) << 12;
              unicode |= (utf8Bytes[pos+4] & 0x3F) << 6;
              unicode |= (utf8Bytes[pos+5] & 0x3F);
              unicodeStr+= String.fromCharCode(unicode) ;
              pos += 6;

          }else if ((flag &0xF8) === 0xF8 ){
              unicode = (utf8Bytes[pos] & 0x7) << 24;
              unicode |= (utf8Bytes[pos+1] & 0x3F) << 18;
              unicode |= (utf8Bytes[pos+2] & 0x3F) << 12;
              unicode |= (utf8Bytes[pos+3] & 0x3F) << 6;
              unicode |= (utf8Bytes[pos+4] & 0x3F);
              unicodeStr+= String.fromCharCode(unicode) ;
              pos += 5;

          } else if ((flag &0xF0) === 0xF0 ){
              unicode = (utf8Bytes[pos] & 0xF) << 18;
              unicode |= (utf8Bytes[pos+1] & 0x3F) << 12;
              unicode |= (utf8Bytes[pos+2] & 0x3F) << 6;
              unicode |= (utf8Bytes[pos+3] & 0x3F);
              unicodeStr+= String.fromCharCode(unicode) ;
              pos += 4;

          } else if ((flag &0xE0) === 0xE0 ){
              unicode = (utf8Bytes[pos] & 0x1F) << 12;;
              unicode |= (utf8Bytes[pos+1] & 0x3F) << 6;
              unicode |= (utf8Bytes[pos+2] & 0x3F);
              unicodeStr+= String.fromCharCode(unicode) ;
              pos += 3;

          } else if ((flag &0xC0) === 0xC0 ){ //110
              unicode = (utf8Bytes[pos] & 0x3F) << 6;
              unicode |= (utf8Bytes[pos+1] & 0x3F);
              unicodeStr+= String.fromCharCode(unicode) ;
              pos += 2;

          } else{
              unicodeStr+= String.fromCharCode(utf8Bytes[pos]);
              pos += 1;
          }
      }
      return unicodeStr;
  }
  }
}
</script>
<style scoped>
  .el-table,.el-button,.el-tag{
    font-size: 16px!important;
  }
</style>

