<template>
	<view>
		<u-navbar :custom-back="back" title="账户">
			<view class="u-navbar-right" slot="right">
				<u-icon name="plus" @tap="toCreate"></u-icon>
			</view>
		</u-navbar>
		<u-card title="账户" :sub-title="totalBalance">
			<view class="" slot="body">
				<view v-for="(item,index) in accounts" :key="index" @tap="toUpdate(item.id)"
					class="u-body-item u-flex u-row-between u-p-b-0">
					<view class="u-body-item-title u-line-2">{{item.title}}</view>
					<view class="">{{item.balance}} ￥</view>
				</view>
			</view>
		</u-card>
	</view>
</template>

<script>
	import {
		request
	} from '@/api/service-base'
	export default {
		name: 'accounts',
		onShow() {
			request({
				url: '/api/app/account?SkipCount=0&MaxResultCount=10'
			}).then(result => {
				console.log(result)
				this.accounts = result.data.items
			})
		},
		data() {
			return {
				accounts: [],
				flatArea: {
					x: 0,
					y: 0
				}
			}
		},
		computed: {
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
				console.log('gogogo')
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
