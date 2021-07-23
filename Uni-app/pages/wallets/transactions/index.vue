<template>
	<view>
		<u-navbar title="交易">
			<view class="u-navbar-right" slot="right">
				<u-icon name="plus" @tap="toCreate"></u-icon>
			</view>
		</u-navbar>
		<u-cell-group>
			<u-cell-item v-for="(item,index) in list" :key="index"
				:title="item.note || (item.category ? item.category.title : item.transactionTypeDescription)"
				:label="item.date" @tap="toUpdate(item.id)">
				<u-avatar class="category-icon" text=' ' :bg-color="item.category ? numberToColor(item.category.color) : 'gray'" slot="icon">
				</u-avatar>
				<view class="u-body-item-title u-line-2">{{item.transactionType === 1?'-':''}}{{item.amount}}
					{{item.symbol}}
				</view>
				<u-tag v-for="(tag, tindex) in item.tags" :key="tindex" type="info" size="mini" :text="tag.title">
				</u-tag>
			</u-cell-item>
		</u-cell-group>
	</view>
</template>

<script>
	import Color from '@/common/color'
	import {
		request
	} from '@/api/service-base'
	export default {
		name: 'accounts',
		onReachBottom() {

		},
		onShow() {
			this.load(true)
		},
		data() {
			return {
				page: 1,
				max: 20,
				list: [],
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
			numberToColor(val) {
				if (val) {
					return '#' + Color.numberToHex(val)
				}
				return ''
			},
			toCreate() {
				uni.navigateTo({
					url: '/pages/wallets/transactions/create'
				})
			},
			toUpdate(id) {
				uni.navigateTo({
					url: `/pages/wallets/transactions/update?id=${id}`
				})
			},
			load(isRefresh = false) {
				if (isRefresh) {
					this.page = 1
				}
				request({
					url: `/api/app/transaction?MaxResultCount=${this.max}&SkipCount=${this.max*(this.page-1)}`
				}).then(result => {
					this.list = result.data.items.map(it => {
						if (it.accountFrom && it.accountFrom.currency) {
							it.symbol = it.accountFrom.currency.symbol
						} else if (it.accountTo && it.accountTo.currency) {
							it.symbol = it.accountTo.currency.symbol
						} else {
							it.symbol = '￥'
						}
						it.date = it.date ? it.date.split('T')[0] : ''
						return it
					})
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
	
	.category-icon{
		margin-right: 20rpx;
	}

	.u-body-item image {
		width: 120rpx;
		flex: 0 0 120rpx;
		height: 120rpx;
		border-radius: 8rpx;
		margin-left: 12rpx;
	}
</style>
