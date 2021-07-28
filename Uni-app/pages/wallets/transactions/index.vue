<template>
	<view>
		<u-navbar :customBack="back" title="交易">
			<view class="u-navbar-right" slot="right">
				<u-icon name="plus" @tap="toCreate"></u-icon>
			</view>
		</u-navbar>
		<u-cell-group>
			<u-cell-item v-for="(item,index) in list" :key="index" @tap="toUpdate(item.id)">
				<u-avatar class="category-icon" text=' '
					:bg-color="item.category ? numberToColor(item.category.color) : 'gray'" slot="icon">
				</u-avatar>
				<view slot="title" class="line-more">
					{{item.note || (item.category ? item.category.title : item.transactionTypeDescription)}}
				</view>
				<view class="u-body-item-title">{{item.transactionType === 1?'-':''}}{{item.amount}}
					{{item.symbol}}
				</view>
				<view slot="label">
					{{$u.timeFormat(item.date, 'yyyy-mm-dd hh:MM')}}
					<view class="m-l-15"></view>
					<u-tag v-for="(tag, tindex) in item.tags" :key="tindex" type="info" size="mini" :text="tag.title">
					</u-tag>
				</view>
			</u-cell-item>
			<u-loadmore :status="status" />
		</u-cell-group>
		<u-back-top :scroll-top="scrollTop"></u-back-top>
	</view>
</template>

<script>
	import Color from '@/common/color'
	import {
		request
	} from '@/api/service-base'
	export default {
		onShow() {
			this.load(true)
		},
		onPageScroll(e) {
			this.scrollTop = e.scrollTop;
		},
		data() {
			return {
				status: 'loadmore',
				page: 1,
				max: 20,
				list: [],
				scrollTop: 0,
			}
		},
		computed: {
			skip() {
				return this.page * this.max
			},
			totalBalance() {
				let val = 0.0
				for (const account of this.accounts) {
					val += account.balance
				}
				return (val).toString().indexOf('.') ? (val).toFixed(2) : val
			}
		},
		methods: {
			numberToColor(val) {
				if (val) {
					return '#' + Color.numberToHex(Math.abs(val))
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
			onReachBottom() {
				this.status = 'loading';
				this.page += 1;
				this.load()
			},
			load(isRefresh = false) {
				if (isRefresh) {
					this.page = 1
				}
				this.status = 'loading'
				request({
					url: `/api/app/transaction?MaxResultCount=${this.max}&SkipCount=${this.max*(this.page-1)}`
				}).then(result => {
					result.data.items.map(it => {
						if (it.accountFrom && it.accountFrom.currency) {
							it.symbol = it.accountFrom.currency.symbol
						} else if (it.accountTo && it.accountTo.currency) {
							it.symbol = it.accountTo.currency.symbol
						} else {
							it.symbol = '￥'
						}
						it.date = it.date ? it.date.split('T')[0] : ''
						this.list.push(it)
					})
					this.status = result.items.length >= this.max ? 'loadmore' : 'nomore'
				}).catch(err => {
					this.status = 'loadmore'
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
	.line-more {
		width: 27vh;
		overflow: hidden;
		text-overflow: ellipsis;
		white-space: nowrap;
	}

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

	.category-icon {
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
