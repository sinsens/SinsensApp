<template>
	<view>
		<u-navbar title="新建账户"></u-navbar>
		<u-card title="账户">
			<view class="" slot="body">
				<u-form :model="model" ref="uForm">
					<u-form-item label="主题" prop="title">
						<u-input v-model="model.title" required placeholder="主题" maxlength="32" />
					</u-form-item>
					<u-form-item label="余额" prop="balance">
						<u-input type="number" v-model="model.balance" placeholder="账户余额" />
					</u-form-item>
					<u-form-item label="货币" prop="currencyCode">
						<u-select v-model="show" :list="currenciesItems" @confirm="selectCurrency"></u-select>
						<view @tap="show = true">{{ model.currencyCode||'选择' }}</view>
					</u-form-item>
					<u-form-item label="包含在总计" prop="includeInTotals">
						<u-checkbox v-model="model.includeInTotals">是</u-checkbox>
					</u-form-item>
					<u-form-item label="备注" prop="note">
						<u-input type="textarea" v-model="model.note" required placeholder="备注" maxlength="4000" />
					</u-form-item>
				</u-form>
			</view>
			<view class="" slot="foot">
				<u-button @click="submit" type="primary">保存</u-button>
				<view class="height-15"></view>
				<u-button @click="back">取消</u-button>
			</view>
		</u-card>
	</view>
</template>

<script>
	import {
		request
	} from '@/api/service-base'
	import {
		AccountCreateUpdateDto
	} from '@/api/service-proxies'
	export default {
		data() {
			return {
				show: false,
				model: new AccountCreateUpdateDto(),
				currencies: [],
				currenciesItems: [],
				rules: {
					title: {
						required: true,
						message: '请输入主题'
					},
					currencyCode: {
						required: true,
						message: '请选择货币'
					},
					balance: [{
						required: true,
						message: '请输入金额'
					}, {
						type: 'number',
						message: '请输入正确的金额'
					}]
				}
			}
		},
		mounted() {
			request({
				url: '/api/app/currency?SkipCount=0&MaxResultCount=10'
			}).then(result => {
				console.log(result)
				this.currencies = result.data.items
				const list = []
				result.data.items.map(it => {
					list.push({
						label: `${it.code} ${it.symbol}`,
						value: it.code
					})
				})
				this.currenciesItems = list
			})
		},
		onReady() {
			this.model.balance = '0'
			this.$refs.uForm.setRules(this.rules);
		},
		methods: {
			keyboardChange(val) {
				this.model.balance = (this.model.balance || '0').toString()
				if (this.model.balance === '0' && val != '.') {
					this.model.balance = val
				} else {
					this.model.balance += val
				}
			},
			backspace() {
				let value = (this.model.balance || '').toString()
				if (value.length) {
					value = value.substr(0, value.length - 1)
				}
				this.model.balance = value || '0'
				console.log(value);
			},
			selectCurrency(e) {
				if (e) {
					this.model.currencyCode = e[0].value
				}
			},
			back() {
				uni.redirectTo({
					url: './index'
				})
			},
			submit() {
				const that = this
				this.$refs.uForm.validate(valid => {
					if (valid) {
						console.log('验证通过')
						request({
							url: '/api/app/account',
							method: 'post',
							data: this.model
						}).then(res => {
							if (res.data.id)
								uni.showModal({
									title: '提示',
									content: '保存成功！',
									showCancel: false,
									success(r) {
										const t = setTimeout(() => {
											uni.navigateBack()
										}, 1200)
									}
								})
						}).catch(err => {
							uni.showToast({
								title: err.error_description || err.error || err || '保存失败',
								icon: 'none'
							})
						})
					} else {
						console.log('验证失败')
						return
					}
				});
			}
		}
	}
</script>

<style scoped lang="scss">
	.keyboard-tip {
		padding-left: 40rpx;
	}
</style>
