/// <reference types="sketch.d.ts"/>
import { SketchProcess } from './sketchProcess'

exports.startSketchProcess = function(context: SketchContext): void
{	
	//パーサーの構築
	const sketchProcess = new SketchProcess();
	sketchProcess.start();
	
}
