<template>
	<view>
		<u-navbar isBack={false} title="更新交易"></u-navbar>
		<u-card title="交易">
			<view class="" slot="body">
				<u-form :model="model" ref="uForm">
					<u-form-item label="金额" prop="amount">
						<u-input type="text" v-model="model.amount"
							@tap="showKeyboard = true;keyboardValue = model.amount" required placeholder="交易金额" />
						<u-keyboard mode="number" @backspace="backspace" @change="keyboardChange"
							v-model="showKeyboard">
							<view class="keyboard-tip" slot="default">当前输入内容：{{keyboardValue}}</view>
						</u-keyboard>
					</u-form-item>
					<u-form-item label="交易分类" prop="category">
						<view @tap="show = true">{{ model.category && model.category.title ||'选择' }}</view>
						<u-modal title="请选择分类" :show-confirm-button="false" v-model="show">
							<CategoryPicker :show="show" @select="selectCategory"></CategoryPicker>
						</u-modal>
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
	import CategoryPicker from '@/components/category-picker'
	import {
		request,
		requestPut
	} from '@/api/service-base'
	export default {
		components: {
			CategoryPicker
		},
		data() {
			return {
				model: {},
				show: false,
				showKeyboard: false,
				keyboardValue: '',
				categories: [],
				rules: {
					amount: {
						required: true,
						message: '请输入交易金额'
					},
					data: {
						required: true,
						message: '请选择交易时间'
					},
					category: {
						required: true,
						message: '请选择交易分类'
					}
				}
			}
		},
		onLoad(options) {
			request({
				url: `/api/app/transaction/${options.id}`
			}).then(res => {
				this.model = res.data
			})
		},
		onReady() {
			this.$refs.uForm.setRules(this.rules)
		},
		mounted() {
			request({
				url: '/api/app/category?MaxResultCount=100'
			}).then(result => {
				const list = [{
					labal: '收入',
					value: 2,
					children: []
				}, {
					labal: '支出',
					value: 1,
					children: []
				}, {
					label: '转账',
					value: 3,
					children: []
				}]
				result.data.items.map(it => {
					const child = {
						labal: it.title,
						value: it
					}
					const parent = list.find(x => x.value == it.transactionType)
					if (parent) {
						parent.children.push(child)
					}
				})
				console.log(list)
				this.categories = list
			})
		},
		methods: {
			keyboardChange(val) {
				this.keyboardValue += val
				this.model.balance = this.keyboardValue
			},
			backspace() {
				if (typeof(this.keyboardValue) == 'number') {
					this.keyboardValue = (this.keyboardValue).toString()
				}
				if (this.keyboardValue.length) this.keyboardValue = this.keyboardValue.substr(0, this.keyboardValue
					.length - 1);
				this.model.balance = this.keyboardValue
				console.log(this.keyboardValue);
			},
			selectCategory(e) {
				console.log('selectCategory', e)
				this.model.category = e
				this.model.transactionType = e.transactionType
				this.show = false
			},
			submit() {
				const that = this
				this.$refs.uForm.validate(valid => {
					if (valid) {
						console.log('验证通过')
						request({
							url: `/api/app/transaction/${this.model.id}`,
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
			},
			back() {
				uni.navigateBack()
			}
		}
	}
</script>

<style>
</style>
