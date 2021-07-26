export default class Color {
	static rgbToHex(r, g, b) {
		return '0x' + (((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1))
	}

	static hexToNumber(hex) {
		return 16581375 - parseInt(hex)
	}

	static numberToHex(num) {
		return (16581375 - Math.abs(num || 0)).toString(16)
	}

	static rgbToNumber(r, g, b) {
		return Color.hexToNumber(Color.rgbToHex(r, g, b))
	}
}
