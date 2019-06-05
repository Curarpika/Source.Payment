<template>
  <div :class="className" :style="{height:height,width:width}" />
</template>

<script>
import echarts from 'echarts'
require('echarts/theme/macarons') // echarts theme
import resize from './mixins/resize'

export default {
  mixins: [resize],
  props: {
    className: {
      type: String,
      default: 'chart'
    },
    width: {
      type: String,
      default: '100%'
    },
    height: {
      type: String,
      default: '500px'
    },
    autoResize: {
      type: Boolean,
      default: true
    },
    chartData: {
      type: Object,
      required: true
    },
    lineChartTitle:{
      type: String,
      default: ''
    }
  },
  data() {
    return {
      chart: null
    }
  },
  watch: {
    chartData: {
      deep: true,
      handler(val) {
        this.setOptions(val)
      }
    }
  },
  mounted() {
    this.$nextTick(() => {
      this.initChart()
    })
  },
  beforeDestroy() {
    if (!this.chart) {
      return
    }
    this.chart.dispose()
    this.chart = null
  },
  methods: {
    initChart() {
      this.chart = echarts.init(this.$el, 'macarons')
      this.setOptions(this.chartData)
    },
    setOptions({ label, creditAddedValue, cashUsedValue } = {}) {
      if (!label || !creditAddedValue|| !cashUsedValue) {
        return
      }
      this.chart.setOption({
        title : {
          text: `消费金额统计 ${this.lineChartTitle}`
        },
        toolbox: {
        show : true,
        feature : {
            mark : {show: true},
            dataView : {show: true, readOnly: false},
            magicType : {show: true, type: ['line', 'bar']},
            restore : {show: true},
            saveAsImage : {show: true}
        }
    },
        xAxis: {
          data: label,
          boundaryGap: false,
          axisTick: {
            show: false
          }
        },
        grid: {
          left: 10,
          right: 10,
          bottom: 20,
          top: 30,
          containLabel: true
        },
        tooltip: {
          trigger: 'axis',
          axisPointer: {
            type: 'cross'
          },
          padding: [5, 10]
        },
        yAxis: {
          axisTick: {
            show: false
          },
          axisLabel : {
            formatter: '{value} 元'
          }
        },
        legend: {
          data: ['充值金额','现金支付']
        },
        series: [{
          name: '充值金额', itemStyle: {
            normal: {
              color: '#40c9c6',
              lineStyle: {
                color: '#40c9c6',
                width: 2
              }
            }
          },
          smooth: true,
          type: 'line',
          data: creditAddedValue,
          animationDuration: 2800,
          animationEasing: 'cubicInOut'
        },
          {
          name: '现金支付',
          smooth: true,
          type: 'line',
          itemStyle: {
            normal: {
              color: '#f4516c',
              lineStyle: {
                color: '#f4516c',
                width: 2
              }
            }
          },
          data: cashUsedValue,
          animationDuration: 2800,
          animationEasing: 'quadraticOut'
        }]
      })
    }
  }
}
</script>
