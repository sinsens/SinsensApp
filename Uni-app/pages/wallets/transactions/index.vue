<template>
	<view>
		<u-navbar :custom-back="back" title="交易">
			<view class="u-navbar-right" slot="right">
				<u-icon name="plus" @tap="toCreate"></u-icon>
			</view>
		</u-navbar>
		<u-card title="交易" sub-title="">
			<view class="" slot="body">
				<view v-for="(item,index) in transactions" :key="index"
					class="u-body-item u-flex u-row-between u-p-b-0">
					<view class="u-body-item-title u-line-2">{{item.note}}</view>
					<view class="">{{item.amount}} {{item.currencyCode}}</view>
				</view>
			</view>
		</u-card>
	</view>
</template>

<script>
	import { request } from '@/api/service-base'
	export default {
		name: 'accounts',
		onShow() {
			request({
				url:'/api/app/transaction?'
			})
		},
		data() {
			return {
				page: 1,
				max: 10,
				list: [],
				flatArea: {
					x: 0,
					y: 0
				}
			}
		},
		computed: {
			skip() {
				return this.page * this.max
			},
			totalBalance() {
				let val = 0
				for (const account of this.accounts) {
					val += account.balance
				}
				return val + ' ￥'
			}
		},
		methods: {
			toCreate() {
				uni.navigateTo({
					url: './create'
				})
			},
			toUpdate(id) {
				uni.navigateTo({
					url: `/pages/wallets/accounts/update?id=${id}`
				})
			},
			back() {
				uni.reLaunch({
					url: '/pages/wallets/index'
				})
			}
		}
	}
</script>

<style scoped lang="scss">
	.u-navbar-right {
		margin-right: 20px;
	}

	.u-card-wrap {
		background-color: $u-bg-color;
		padding: 1px;
	}

	.u-body-item {
		font-size: 32rpx;
		color: #333;
		padding: 20rpx 10rpx;
	}

	.u-body-item image {
		width: 120rpx;
		flex: 0 0 120rpx;
		height: 120rpx;
		border-radius: 8rpx;
		margin-left: 12rpx;
	}
</style>
