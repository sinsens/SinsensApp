<template>
	<view>
		<u-navbar :isBack="false" title="总览">
			<view slot="right" class="u-navbar-right-icon">
				<u-icon name="list" @click="showLeftMenu=true"></u-icon>
			</view>
		</u-navbar>
		<u-popup v-model="showLeftMenu" mode="right">
			<u-cell-group class="left-menu-container">
				<u-cell-item v-for="(item, index) in menus" :key="index" @click="toUrl(item.path)">
					<u-icon slot="icon" :name="item.iconPath"></u-icon>
					{{item.text}}
				</u-cell-item>
			</u-cell-group>
		</u-popup>
		<u-card @tap="toAccounts" title="账户" :sub-title="'包含在总计中 ' + total + '￥' ">
			<view class="" slot="body">
				<view v-for="(item,index) in accounts.filter(x=>x.includeInTotals)" :key="index"
					class="u-body-item u-flex u-row-between u-p-b-0">
					<view class="u-body-item-title u-line-2">{{item.title}}</view>
					{{item.balance}} {{item.currency ? item.currency.symbol : '￥'}}
				</view>
			</view>
		</u-card>

		<u-card @tap="toTransactions" title="交易" sub-title="包含在总计中">
			<view class="" slot="body">
				<view v-for="(item,index) in transactions" :key="index"
					class="u-body-item u-flex u-row-between u-p-b-0">
					<view class="u-body-item-title u-line-2">{{item.note}}</view>
					{{item.transactionType === 1?'-':''}}{{item.amount}} {{item.symbol}}
				</view>
			</view>
		</u-card>
	</view>
</template>

<script>
	import Color from '@/common/color'
	import {
		request
	} from '@/api/service-base'
	export default {
		onShow() {
			request({
				url: '/api/app/account?SkipCount=0&MaxResultCount=100'
			}).then(result => {
				console.log(result)
				this.accounts = result.data.items
			})
			request({
				url: '/api/app/transaction?SkipCount=0&MaxResultCount=10'
			}).then(result => {
				console.log(result)
				this.transactions = result.data.items.map(it => {
					if (it.accountFrom && it.accountFrom.currency) {
						it.symbol = it.accountFrom.currency.symbol
					} else if (it.accountTo && it.accountTo.currency) {
						it.symbol = it.accountTo.currency.symbol
					} else {
						it.symbol = '￥'
					}
					return it
				})
			})
		},
		data() {
			return {
				showLeftMenu: false,
				accounts: [],
				transactions: [],
				menus: [{
						iconPath: "home",
						selectedIconPath: "home-fill",
						text: '总览',
						path: '/pages/wallets/index'
					}, {
						iconPath: "grid",
						selectedIconPath: "home-fill",
						text: '报表',
						path: '/pages/wallets/report/report'
					},
					{
						iconPath: "account",
						text: '账户',
						path: '/pages/wallets/accounts/index'
					},
					{
						iconPath: "order",
						text: '交易',
						path: '/pages/wallets/transactions/index'
					},
					{
						iconPath: "grid",
						text: '分类',
						path: '/pages/wallets/categories/index'
					},
					{
						iconPath: "tags",
						text: '标签',
						path: '/pages/wallets/tags/index'
					}, {
						iconPath: 'download',
						text: '备份',
						path: '/pages/wallets/backup/index'
					}
				],
				current: 0
			}
		},
		computed: {
			total: function() {
				let tmp = 0.0;
				this.accounts.map((it, index) => {
					if (it.includeInTotals)
						tmp += it.balance
				})
				return (tmp).toString().indexOf('.') ? (tmp).toFixed(2) : tmp
			}
		},
		methods: {
			numberToColor(val) {
				return (val != undefined && val != null) ? '#' + Color.numberToHex(val) : 'white'
			},
			toUrl(url) {
				wx.navigateTo({
					url: url
				})
			},
			toTransactions() {
				this.toUrl('/pages/wallets/transactions/index')
			},
			toAccounts() {
				this.toUrl('/pages/wallets/accounts/index')
			}
		}
	}
</script>

<style scoped lang="scss">
	.info-color {
		color: $u-type-info-dark
	}

	.u-navbar-right-icon {
		margin-right: 40rpx;
	}

	.left-menu-container {
		margin-top: 80rpx;
		width: 200rpx;
	}

	.color-preview {
		display: inline-block;
		vertical-align: bottom;
		margin-right: 20rpx;
		width: 40rpx;
		height: 40rpx;
	}

	.u-body-item {
		font-size: 32rpx;
		color: #333;
		padding: 20rpx 10rpx;
	}
</style>
