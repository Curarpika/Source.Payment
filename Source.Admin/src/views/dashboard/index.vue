<template>
  <div class="dashboard-editor-container">
    <el-button-group>
      <el-button @click="handleFilter('day')">今日</el-button>
      <el-button @click="handleFilter('week')">本周</el-button>
      <el-button @click="handleFilter('month')">本月</el-button>
    </el-button-group>

    <el-date-picker
      v-model="pickerDate"
      type="daterange"
      align="center"
      unlink-panels
      range-separator="至"
      start-placeholder="开始日期"
      end-placeholder="结束日期"
      :picker-options="pickerOptions">
    </el-date-picker>

    <el-button type="primary" @click="handleFilter('custom')">查询</el-button>

    <panel-group  :chart-data="lineChartData"/>

    <el-row style="background:#fff;padding:16px 16px 0;margin-bottom:32px;">
      <line-chart :chart-data="lineChartData" :line-chart-title="lineChartTitle"/>
    </el-row>

  </div>
</template>

<script>
import PanelGroup from './components/PanelGroup'
import LineChart from './components/LineChart'
import { dashboard } from '@/api/payment-order'


export default {
  name: 'DashboardAdmin',
  components: {
    PanelGroup,
    LineChart
  },
  data() {
    return {
      dateStart:'2019-05-30',
      dateEnd:'2019-06-03',
      lineChartData: {},
      lineChartTitle:'',
      pickerOptions: {
          shortcuts: [{
            text: '最近7天',
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              picker.$emit('pick', [start, end]);
            }
          }, {
            text: '最近30天',
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              picker.$emit('pick', [start, end]);
            }
          }]
        },
        pickerDate: ''
    }
  },
  created() {
    this.handleFilter('day')
  },
  methods: {
    getList() {
      dashboard({dateStart:this.dateStart,dateEnd:this.dateEnd}).then(res=>{
        this.lineChartData = res.data
      })  
    },
    handleFilter(preset) {
      var today = new Date();
 
      if(preset === 'day'){
        this.dateStart = today.toLocaleDateString()
        this.dateEnd = today.toLocaleDateString()
        this.lineChartTitle = '今日'
      }
      else if(preset === 'week'){
        // 本周一
        today.setDate(today.getDate() - today.getDay() + 1);
        this.dateStart = today.toLocaleDateString()
      
        // 本周日
        today.setDate(today.getDate() + 6);
        this.dateEnd = today.toLocaleDateString()
        this.lineChartTitle = '本周'
      }
      else if (preset === 'month'){
        // 本月初
        today.setDate(1);
        this.dateStart = today.toLocaleDateString()
      
        // 本月末
        var currentMonth=today.getMonth();
        var nextMonth=++currentMonth;
        var nextMonthFirstDay=new Date(today.getFullYear(),nextMonth,1);
        var oneDay=1000*60*60*24;
        this.dateEnd  = new Date(nextMonthFirstDay-oneDay).toLocaleDateString()
        this.lineChartTitle = '本月'
      }
      else if(preset === 'custom'){
        if(this.pickerDate.length ===0){
          this.$message({
            message: '请选择时间',
            type: 'warning'
          })
          return
        }
        this.dateStart = this.pickerDate[0].toLocaleDateString()
        this.dateEnd = this.pickerDate[1].toLocaleDateString()
        this.lineChartTitle = `${this.dateStart.replace(new RegExp('/','g'),'-')} - ${this.dateEnd.replace(new RegExp('/','g'),'-')}`
      }

      this.getList()
    }
  }
}
</script>

<style lang="scss" scoped>
.dashboard-editor-container {
  padding: 32px;
  background-color: rgb(240, 242, 245);
  position: relative;

  .github-corner {
    position: absolute;
    top: 0px;
    border: 0;
    right: 0;
  }

  .chart-wrapper {
    background: #fff;
    padding: 16px 16px 0;
    margin-bottom: 32px;
  }
}

@media (max-width:1024px) {
  .chart-wrapper {
    padding: 8px;
  }
}
</style>
