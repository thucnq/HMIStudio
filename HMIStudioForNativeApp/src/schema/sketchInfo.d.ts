declare module '*/sketchInfoCommon.json'
{
	//typedoc中のtypescript 2.7.2 だと下記構文でエラーになるので暫定的にanyとする
	const value: any;//import('../types').SketchInfo;
	export = value;
}
