package com.ort.studyup.common.renderers

import android.graphics.Typeface
import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_result.view.*
import kotlin.math.roundToInt

class ResultItemRenderer() : ViewRenderer<ResultItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_result, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.username.text = model.username
        view.position.text = model.position.toString()
        view.score.text = ((model.score * 10).roundToInt() / 10.0).toString()
        if (model.bold) {
            //TODO: make more visible
            view.position.setTypeface(view.username.typeface, Typeface.BOLD)
        } else {
            view.position.setTypeface(view.username.typeface, Typeface.NORMAL)
        }
    }

    class Item(
            val position: Int,
            val username: String,
            val score: Double,
            val bold: Boolean = false
    )
}