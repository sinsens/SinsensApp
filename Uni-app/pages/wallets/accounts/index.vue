<template>
	<view>
		<u-navbar :customBack="back" title="账户">
			<view class="u-navbar-right" slot="right">
				<u-icon name="plus" @tap="toCreate"></u-icon>
			</view>
		</u-navbar>
		<u-card title="账户" :sub-title="totalBalance">
			<view class="" slot="body">
				<view v-for="(item,index) in accounts" :key="index" @tap="toUpdate(item.id)"
					class="u-body-item u-flex u-row-between u-p-b-0">
					<view :class="'u-body-item-title u-line-2 ' + (item.includeInTotals? '' : ' info-color')">
						{{item.title}}</view>
					<view :class="(item.includeInTotals? '' : ' info-color')">{{item.balance}} {{item.currency.symbol}}
					</view>
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
				url: '/api/app/account?SkipCount=0&MaxResultCount=100'
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
					if (account.includeInTotals)
						val += account.balance
				}
				return ((val).toString().indexOf('.') ? (val).toFixed(2) : val) + '￥'
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
			back(){
				uni.reLaunch({
					url: '/pages/wallets/index'
				})
			}
		}
	}
</script>

<style scoped lang="scss">
	.info-color {
		color: $u-type-info-dark
	}

	.u-navbar-right {
		margin-right: 20px;
	}

	.u-body-item {
		font-size: 32rpx;
		color: #333;
		padding: 20rpx 10rpx;
	}
</style>
