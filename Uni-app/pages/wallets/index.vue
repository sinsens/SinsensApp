<template>
	<view>
		<u-navbar :isBack="false" :title="maintitle"></u-navbar>
		<view class="u-page">
			<view v-if="current == 0">
				<u-card @tap="toAccounts" title="账户" :sub-title="'包含在总计中 ' + total + '￥' ">
					<view class="" slot="body">
						<view v-for="(item,index) in accounts" :key="index"
							class="u-body-item u-flex u-row-between u-p-b-0">
							<view class="u-body-item-title u-line-2">{{item.title}}</view>
							<view class="">
								{{item.balance}} ￥
							</view>
						</view>
					</view>
				</u-card>

				<u-card @tap="toTransactions" title="交易" sub-title="包含在总计中">
					<view class="" slot="body">
						<view v-for="(item,index) in transactions" :key="index"
							class="u-body-item u-flex u-row-between u-p-b-0">
							<view class="u-body-item-title u-line-2">{{item.note}}</view>
							<view class="">
								{{item.transactionType === 1?'-':''}}{{item.amount}} ￥
							</view>
						</view>
					</view>
				</u-card>
			</view>
			<view v-if="current == 1">
				<u-card @tap="toAccounts" title="账户" :sub-title="'包含在总计中 ' + total + '￥' ">
					<view class="" slot="body">
						<view v-for="(item,index) in accounts" :key="index"
							class="u-body-item u-flex u-row-between u-p-b-0">
							<view class="u-body-item-title u-line-2">{{item.title}}</view>
							<view class="">{{item.balance}} ￥</view>
						</view>
					</view>
				</u-card>
			</view>
			<view v-if="current == 2">
				<u-card @tap="toTransactions" title="交易" sub-title="">
					<view class="" slot="body">
						<view v-for="(item,index) in transactions" :key="index"
							class="u-body-item u-flex u-row-between u-p-b-0">
							<view class="u-body-item-title u-line-2">{{item.note}}</view>
							<view class="">
								{{item.transactionType === 1?'-':''}}{{item.amount}} ￥
							</view>
						</view>
					</view>
				</u-card>
			</view>
			<view v-if="current == 3">
				<u-card @tap="toCategories" title="分类" sub-title="">
					<view class="" slot="body">
						<view v-for="(item,index) in categories" :key="index"
							class="u-body-item u-flex u-row-between u-p-b-0">
							<view class="">
								{{item.title}}
							</view>
						</view>
					</view>
				</u-card>
			</view>
			<view v-if="current == 4">
				<u-card @tap="toTags" title="标签" sub-title="">
					<view class="" slot="body">
						<view v-for="(item,index) in tags" :key="index"
							class="u-body-item u-flex u-row-between u-p-b-0">
							<view class="">
								{{item.title}}
							</view>
						</view>
					</view>
				</u-card>
			</view>
		</view>
		<!-- 与包裹页面所有内容的元素u-page同级，且在它的下方 -->
		<u-tabbar v-model="current" :list="list" :mid-button="true"></u-tabbar>
	</view>
</template>

<script>
	import {
		request
	} from '@/api/service-base'
	export default {
		onShow() {
			request({
				url: '/api/app/account?SkipCount=0&MaxResultCount=10'
			}).then(result => {
				console.log(result)
				this.accounts = result.data.items
			})
			request({
				url: '/api/app/transaction?SkipCount=0&MaxResultCount=10'
			}).then(result => {
				console.log(result)
				this.transactions = result.data.items
			})
			request({
				url: '/api/app/tag?SkipCount=0&MaxResultCount=10'
			}).then(result => {
				console.log(result)
				this.tags = result.data.items
			})
			request({
				url: '/api/app/category?SkipCount=0&MaxResultCount=10'
			}).then(result => {
				console.log(result)
				this.categories = result.data.items
			})
		},
		data() {
			return {
				accounts: [],
				transactions: [],
				tags: [],
				categories: [],
				list: [{
						iconPath: "home",
						selectedIconPath: "home-fill",
						text: '总览',
						customIcon: false,
					},
					{
						iconPath: "grid",
						selectedIconPath: "grid-fill",
						text: '账户',
						customIcon: false,
					},
					{
						iconPath: "order",
						selectedIconPath: "order",
						text: '交易',
						midButton: true,
						customIcon: false,
					},
					{
						iconPath: "play-right",
						selectedIconPath: "play-right-fill",
						text: '分类',
						customIcon: false,
					},
					{
						iconPath: "account",
						selectedIconPath: "account-fill",
						text: '标签',
						count: 23,
						isDot: false,
						customIcon: false,
					},
				],
				current: 0
			}
		},
		computed: {
			maintitle: function() {
				return this.list[this.current].text
			},
			total: function() {
				let tmp = 0.0;
				this.accounts.map((it, index) => {
					tmp += it.balance
				})
				return tmp
			}
		},
		methods: {
			toAccounts() {
				uni.navigateTo({
					url: 'accounts/index'
				})
			},
			toTransactions() {
				uni.navigateTo({
					url: 'transactions/index'
				})
			},
			toCategories() {
				uni.navigateTo({
					url: 'categories/index'
				})
			},
			toTags() {
				uni.navigateTo({
					url: 'tags/index'
				})
			}
		}
	}
</script>
