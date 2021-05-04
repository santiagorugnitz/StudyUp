package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_exam.view.*

class ExamItemRenderer(private val callback: Callback) : ViewRenderer<ExamItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_exam, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        //TODO:
        view.examName.text = model.name
        view.setOnClickListener {
            callback.onExamClicked(model.id)
        }
    }

    class Item(
        val id:Int,
        val name: String,
        val groupName:String?,
    )

    interface Callback {
        fun onExamClicked(examId: Int)
    }
}