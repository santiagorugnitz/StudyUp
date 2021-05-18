package com.ort.studyup.common.renderers

import android.icu.number.Scale.none
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import com.ort.studyup.R
import com.ort.studyup.common.models.ExamItem
import com.ort.studyup.common.models.GroupItem
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_comment.view.*

class CommentRenderer : ViewRenderer<CommentRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_comment, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.username.text = model.name
        view.comment.text = model.comment
        view.date.text = model.date

    }

    class Item(
            val id: Int,
            val comment: String,
            val name: String,
            val date: String,

    )

}