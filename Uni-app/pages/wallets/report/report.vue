<template>
	<view>
		<u-navbar :customBack="back" title="报表">
			<view class="u-navbar-right" slot="right">
			</view>
		</u-navbar>
		<u-card title="统计日期">
			<view slot="body">
				<u-form :label-width="200">
					<u-form-item label="开始日期">
						<u-picker v-model="showStartDate" :params="params" @confirm="selectedStartDate"></u-picker>
						<view @tap="showStartDate = true">{{startDate || '选择'}}</view>
					</u-form-item>
					<u-form-item label="结束日期">
						<u-picker v-model="showEndDate" :params="params" @confirm="selectedEndDate"></u-picker>
						<view @tap="showEndDate = true">{{endDate || '选择'}}</view>
					</u-form-item>
				</u-form>
				<u-button :throttleTime="5000" type="primary" @tap="refresh">查询</u-button>
			</view>
		</u-card>
		<view class="u-page">
			<view class="list-item" v-for="(item,index) in list" :key="index">{{item.text}} {{item.value}} ￥
				<view>
					<view class="info" v-for="(tag, tindex) in item.children" :key="tindex">
						{{tag.text}}
						<view class="tag-item">{{tag.value}} ￥</view>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import Color from '@/common/color'
	import {
		request
	} from '@/api/service-base'
	export default {
		data() {
			return {
				list: [],
				startDate: '2020-01-01',
				endDate: '2021-07-27',
				showEndDate: false,
				showStartDate: false,
				params: {
					year: true,
					month: true,
					day: true,
					hour: false,
					minute: false,
					timestamp: true
				}
			}
		},
		mounted() {
			const now = new Date().getTime()
			this.endDate = this.$u.timeFormat(now)
			this.startDate = this.$u.timeFormat(now - 7*24*3600*1000)
			this.refresh()
		},
		methods: {
			refresh() {
				request({
					url: `/api/app/stat/period-result-list?StartTime=${this.startDate}&EndTime=${this.endDate}`
				}).then(result => {
					this.list = result.data.items.sort((a, b) => b.percent - a.percent)
				})
			},
			selectedStartDate(e) {
				const {
					timestamp
				} = e
				console.log(e)
				this.startDate = this.$u.timeFormat(timestamp)
			},
			selectedEndDate(e) {
				const {
					timestamp
				} = e
				this.endDate = this.$u.timeFormat(timestamp)
			},
			numberToColor(val) {
				if (val) {
					return '#' + Color.numberToHex(Math.abs(val))
				}
				return ''
			},
			back(){
				uni.reLaunch({
					url: '/pages/wallets/index'
				})
			}
		}
	}
</script>

<style lang="scss">
	.list-item {
		padding-bottom: 10rpx;
	}

	.info {
		font-size: 0.9em;
		line-height: 40rpx;
		color: #bbb
	}

	.tag-item {
		float: right;
	}
</style>
