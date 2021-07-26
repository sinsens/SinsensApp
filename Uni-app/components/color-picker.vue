<template name="ColorPicker">
	<view v-if="show">
		<view class="content" :style="{height:'120px',width:'120px',background:hasChange ? color : orignColorStyle }">
		</view>
		<br />
		<u-slider v-model="r" @end="change"></u-slider>
		<br />
		<u-slider v-model="g" @end="change"></u-slider>
		<br />
		<u-slider v-model="b" @end="change"></u-slider>
	</view>
</template>

<script>
	export default {
		props: {
			show: Boolean = false,
			orignColor: Number
		},
		name: "ColorPicker",
		created() {
			if (this.$props.orignColor != undefined) {
				this.orignColorStyle = '#' + this.numberToHex(this.$props.orignColor)
				this.hasChange = false
			} else {
				this.orignColorStyle = (this.color).toString()
				this.hasChange = false
			}

		},
		data() {
			return {
				orignColorStyle: '',
				hasChange: true,
				r: 50,
				g: 50,
				b: 100,
			}
		},
		methods: {
			change() {
				this.hasChange = true
			},
			rgbToHex(r, g, b) {
				return '0x' + (((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1))
			},
			hexToNumber(hex) {
				return 16581375 - parseInt(hex)
			},
			numberToHex(num) {
				return (16581375 - Math.abs(num || 0)).toString(16)
			},
			rgbToNumber(r, g, b) {
				return this.hexToNumber(this.rgbToHex(r, g, b))
			}
		},
		computed: {
			color: function() {
				const rgb = {
					r: Math.round(this.r * 2.55),
					g: Math.round(this.g * 2.55),
					b: Math.round(this.b * 2.55)
				}
				const hex = this.rgbToHex(rgb.r, rgb.g, rgb.b)
				const colour = {
					hex,
					number: this.rgbToNumber(rgb.r, rgb.g, rgb.b),
					color: hex.replace('0x', '#')
				}
				this.hasChange = true
				this.$emit('change', colour)
				return colour.hex.replace('0x', '#')
			}
		}
	}
</script>

<style>

</style>
