<template>
	<u-modal :title="title || '请选择标签, 最多选 3 个'" :show-confirm-button="true" v-model="show">
		<!--u-cell-group>
			<u-cell-item :arrow="false" v-for="(item,index) in tags" :key="index" :title="item.title">
				<checkbox :checked="item.checked" @click="item.checked = !item.checked"></checkbox>
			</u-cell-item>
		</u-cell-group-->
		<view class="u-padding-10">
			<u-tag v-for="(item,index) in tags" :type="item.checked ? 'primary' : 'info'" :text="item.title"
				:key="index" @tap="item.checked = selectedCount < 3 ? !item.checked : false"></u-tag>
		</view>
		<view slot="confirm-button" @tap="submit">保存</view>
	</u-modal>
</template>

<script>
	/**
	 * @property {String} title = 显示标题
	 * @property {String} selectText = 选择按钮标题
	 * @property {Boolean} show = [false] 是否显示
	 * @property {Array} checkedIds = 已选择的标签 
	 */
	import {
		request
	} from '@/api/service-base'
	export default {
		name: "TagPicker",
		props: {
			title: String,
			selectText: String,
			show: Boolean = true,
			checkedIds: Array
		},
		created() {
			request({
				url: '/api/app/tag?SkipCount=0&MaxResultCount=100'
			}).then(result => {
				let {
					checkedIds,
					show
				} = this.$props
				this.tags = result.data.items.map(it => {
					it.checked = checkedIds.some(x => x.id == it.id)
					return it
				})
			})
		},
		data() {
			return {
				tags: []
			}
		},
		computed: {
			selectedCount() {
				return this.tags.filter(x => x.checked).length
			}
		},
		methods: {
			submit() {
				console.info('confirm', this.tags.filter(x => x.checked))
				const checkedList = this.tags.filter(x => x.checked)
				this.$emit('confirm', checkedList)
			}
		}
	}
</script>

<style scoped lang="scss">
	.u-cell {
		padding: 0 26rpx
	}

	.color-preview {
		display: inline-block;
		vertical-align: middle;
		margin-right: 20rpx;
		width: 40rpx;
		height: 40rpx;
	}

	.slot-content {
		padding-left: 50px;
	}
</style>
