<template>
	<view :style="{background: bgcolor || 'white', height: '100%', 'padding-bottom': '20rpx'}">
		<u-navbar title="更新交易"></u-navbar>
		<u-card title="交易">
			<view class="" slot="body">
				<u-form :model="model" ref="uForm" label-width="200">
					<u-form-item label="金额" prop="amount">
						<u-input type="text" v-model="model.amount" placeholder="交易金额" />
					</u-form-item>
					<u-form-item label="交易时间" prop="date">
						<view @tap="showDate = true">{{ $u.timeFormat(model.date, 'yyyy年mm月dd日 hh:MM') ||'选择' }}</view>
						<u-picker v-model="showDate" mode="time" :params="dateParams" @confirm="selectDate">
						</u-picker>
					</u-form-item>
					<u-form-item label="交易分类" prop="category" v-show="model.transactionType <= 2">
						<view @tap="show = true">{{ model.category && model.category.title ||'选择' }}</view>
						<CategoryPicker :show="show" @select="selectCategory"></CategoryPicker>
					</u-form-item>
					<u-form-item label="支出账户" prop="accountFrom"
						v-show="model.transactionType == 1 || model.transactionType == 3">
						<u-select v-model="showAccountFrom" :list="accounts" @confirm="selectAccountFrom">
						</u-select>
						<view @tap="showAccountFrom = true">{{ model.accountFrom && model.accountFrom.title||'选择' }}
						</view>
					</u-form-item>
					<u-form-item label="收入账户" prop="accountTo"
						v-show="model.transactionType == 2 || model.transactionType == 3">
						<u-select v-model="showAccountTo" :list="accounts" @confirm="selectAccountTo">
						</u-select>
						<view @tap="showAccountTo = true">{{ model.accountTo && model.accountTo.title||'选择' }}
						</view>
					</u-form-item>
					<u-form-item label="标签" prop="tags">
						<u-tag v-if="model && model.tags" v-for="(tag, index) in model.tags" :key="tag.id"
							:text="tag.title" @tap="showTags = true">
						</u-tag>
						<view v-if="model && model.tags && model.tags.length < 1" @tap="showTags = true">选择</view>
						<TagPicker :show="showTags" :checkedIds="model.tags" @confirm="selectTags">
						</TagPicker>
					</u-form-item>
					<u-form-item label="是否已确认" prop="transactionState">
						<u-checkbox v-model="model.transactionState">是</u-checkbox>
					</u-form-item>
					<u-form-item label="包含在总计" prop="includeInReports">
						<u-checkbox v-model="model.includeInReports">是</u-checkbox>
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
	import qs from 'qs'
	import Color from '@/common/color'
	import TagPicker from '@/components/tag-picker'
	import CategoryPicker from '@/components/category-picker'
	import {
		request,
		requestPut
	} from '@/api/service-base'

	export default {
		components: {
			TagPicker,
			CategoryPicker
		},
		data() {
			return {
				model: {},
				show: false,
				showDate: false,
				dateParams: {
					year: true,
					month: true,
					day: true,
					hour: true,
					minute: true,
					second: false,
					timestamp: true
				},
				showAccountFrom: false,
				showAccountTo: false,
				showTags: false,
				accounts: [],
				bgcolor: '',
				rules: {
					amount: [{
						required: true,
						message: '请输入金额'
					}, {
						type: 'number',
						message: '请输入正确的金额'
					}],
					date: {
						required: true,
						message: '请选择交易时间'
					},
					category: {
						required: true,
						message: '请选择交易分类'
					},
					accountFrom: {
						required: true,
						message: '请选择支出账户'
					},
					accountTo: {
						required: true,
						message: '请选择收入账户'
					}
				}
			}
		},
		onLoad(options) {
			request({
				url: `/api/app/transaction/${options.id}`
			}).then(res => {
				this.model = res.data
				this.model.amout = (this.model.amount || '0').toString()
				if (this.model.category) {
					this.bgcolor = '#' + Color.numberToHex(this.model.category.color)
				}
			})
			request({
				url: `/api/app/account?MaxResultCount=100`
			}).then(res => {
				const list = []
				res.data.items.map(it => {
					list.push({
						label: it.title,
						value: it
					})
				})
				console.log('accounts', list)
				this.accounts = list
			})
		},
		onReady() {
			this.$refs.uForm.setRules(this.rules)
		},
		mounted() {},
		methods: {
			selectTags(val) {
				this.model.tags = val
				this.showTags = false
			},
			selectDate(e) {
				const {
					year,
					month,
					day,
					hour,
					minute,
					timestamp
				} = e
				this.model.date = this.$u.timeFormat(timestamp, 'yyyy-mm-dd hh:MM')
			},
			selectCategory(e) {
				console.log('selectCategory', e)
				this.model.category = e
				this.model.transactionType = e.transactionType
				this.bgcolor = '#' + Color.numberToHex(e.color)
				this.show = false
			},
			selectAccountFrom(e) {
				this.showAccountFrom = false
				this.model.accountFrom = e[0].value
				this.checkAccount()
			},
			selectAccountTo(e) {
				this.showAccountTo = false
				this.model.accountTo = e[0].value
				this.checkAccount()
			},
			checkAccount() {
				const {
					accountFrom,
					accountTo
				} = this.model
				if (typeof(accountFrom) != 'undefined' && typeof(accountTo) != 'undefined') {
					if ((accountFrom != null && accountTo != null) && accountFrom.id == accountTo.id) {
						this.model.accountFrom = null
						this.model.accountTo = null
						uni.showModal({
							title: '提示',
							content: '支出账户不可与收入账户相同, 请重新选择',
							showCancel: false
						})
					}
				}
			},
			submit() {
				switch (this.model.transactionType) {
					case 1: // 支出
						this.model.accountTo = undefined
						this.rules.accountTo.required = false
						this.rules.accountFrom.required = true
						break

					case 2: // 收入
						this.model.accountFrom = undefined
						this.rules.accountFrom.required = false
						this.rules.accountTo.required = true
						break

					case 3: // 转账
						this.rules.accountFrom.required = true
						this.rules.accountTo.required = true
						this.rules.category.required = false
						break
				}
				const that = this
				this.$refs.uForm.validate(valid => {
					if (valid) {
						console.log('验证通过')
						requestPut({
							url: `/api/app/transaction/${this.model.id}`,
							data: qs.parse(qs.stringify(this.model))
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
