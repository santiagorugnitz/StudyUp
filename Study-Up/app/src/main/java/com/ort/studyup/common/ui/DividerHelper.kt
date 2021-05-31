package com.ort.studyup.common.ui

import android.content.Context
import android.graphics.Canvas
import android.graphics.Rect
import android.view.View
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.RecyclerView
import com.ort.studyup.R
import com.thinkup.easylist.RendererAdapter

class DividerHelper(context: Context, val padding: Int = 0) : RecyclerView.ItemDecoration() {
    private val divider = ContextCompat.getDrawable(context, R.drawable.item_divider)
    private var indexList = mutableListOf<Int>()
    private var lastOneWithoutLine = false

    constructor(indexList: MutableList<Int>, context: Context, padding: Int = 0, lastOneWithoutLine: Boolean = false)
            : this(context, padding) {
        this.indexList = indexList
        this.lastOneWithoutLine = lastOneWithoutLine
    }

    override fun getItemOffsets(outRect: Rect, view: View, parent: RecyclerView, state: RecyclerView.State) {

        val position = parent.getChildAdapterPosition(view)
        if (position == 0) {
            return
        }
        if (indexList.contains(position) && !lastOneWithoutLine) {
            return
        }
        val adapter = parent.adapter as RendererAdapter
        val maxPosition = adapter.getItems().size
        if (maxPosition == position) {
            return
        }

        super.getItemOffsets(outRect, view, parent, state)
        outRect.top = divider?.intrinsicHeight ?: 0
    }
    override fun onDraw(c: Canvas, parent: RecyclerView, state: RecyclerView.State) {
        divider?.let { divider ->
            val left = parent.paddingLeft + padding
            val right = parent.width - parent.paddingRight - padding
            val adapter = parent.adapter as RendererAdapter
            val maxPosition = adapter.getItems().size
            val childCount = parent.childCount
            for (i in 0 until childCount) {
                if (indexList.contains(i)) {
                    continue
                }
                if (maxPosition -1 == i) {
                    return
                }
                val child = parent.getChildAt(i)
                val params = child.layoutParams as RecyclerView.LayoutParams
                val top = child.bottom + params.bottomMargin
                val bottom = top + divider.intrinsicHeight
                divider.setBounds(left, top, right, bottom)
                divider.draw(c)
            }
        }
    }
}