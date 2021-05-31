package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import androidx.annotation.DrawableRes
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.empty_view.view.*

class EmptyViewRenderer : ViewRenderer<EmptyViewRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.empty_view, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.emptyMessage.text = model.text
        view.icon.setImageResource(model.icon)
    }

    class Item(
        val text: String,
        @DrawableRes val icon: Int = R.drawable.ic_sad_face,
    )
}