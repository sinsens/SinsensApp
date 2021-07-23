<template>
	<view>
		<u-navbar title="修改账户"></u-navbar>
		<u-card title="账户">
			<view class="" slot="body">
				<u-form :model="model" ref="uForm">
					<u-form-item label="主题" prop="title">
						<u-input v-model="model.title" required placeholder="主题" maxlength="32" />
					</u-form-item>
					<u-form-item label="余额" prop="balance">
						<u-input type="text" v-model="model.balance" @tap="showKeyboard = true" required
							placeholder="账户余额">￥
						</u-input>
						<u-keyboard mode="number" @backspace="backspace" @change="keyboardChange"
							v-model="showKeyboard">
							<view class="keyboard-tip" slot="default">当前输入内容：{{this.model.balance}}
								{{model.currency && model.currency.symbol || '￥'}}
							</view>
						</u-keyboard>
					</u-form-item>
					<u-form-item label="货币">
						{{model.currency && model.currency.symbol}}
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
		onLoad(options) {
			request({
				url: `/api/app/account/${options.id}`
			}).then(res => {
				this.model = res.data
			})
		},
		data() {
			return {
				show: false,
				model: new AccountCreateUpdateDto(),
				currencies: [],
				rules: {
					title: {
						required: true,
						message: '请输入主题'
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
		onReady() {
			this.$refs.uForm.setRules(this.rules);
		},
		methods: {
			selectCurrency(e) {
				if (e) {
					this.model.currencyCode = e[0].value
				}
			},
			back() {
				uni.navigateBack()
			},
			submit() {
				const that = this
				this.$refs.uForm.validate(valid => {
					if (valid) {
						console.log('验证通过')
						request({
							url: `/api/app/account/${this.model.id}`,
							method: 'put',
							data: this.model
						}).then((res) => {
							console.log(res)
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
						console.log('验证失败');
						return;
					}
				})
			}
		}
	}
</script>

<style scoped lang="scss">
	.keyboard-tip {
		padding-left: 40rpx;
	}
</style>
