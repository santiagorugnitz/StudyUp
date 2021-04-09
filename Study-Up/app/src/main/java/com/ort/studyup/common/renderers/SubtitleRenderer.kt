package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_subtitle.view.*

class SubtitleRenderer : ViewRenderer<SubtitleRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_subtitle, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.text.text = model.text
    }

    class Item(
        val text: String
    )

}