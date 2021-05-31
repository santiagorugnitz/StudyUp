package com.ort.studyup.common.ui

import android.graphics.Canvas
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.ItemTouchHelper
import androidx.recyclerview.widget.RecyclerView
import com.ort.studyup.R

class SwipeCallback(val callback: Callback) : ItemTouchHelper.SimpleCallback(0, ItemTouchHelper.LEFT or ItemTouchHelper.RIGHT) {

    override fun onMove(recyclerView: RecyclerView, viewHolder: RecyclerView.ViewHolder, target: RecyclerView.ViewHolder): Boolean {
        return false
    }

    override fun onSwiped(viewHolder: RecyclerView.ViewHolder, direction: Int) {
        callback.onSwipe(viewHolder.adapterPosition)
    }

    override fun onChildDraw(
        c: Canvas,
        recyclerView: RecyclerView,
        viewHolder: RecyclerView.ViewHolder,
        dX: Float,
        dY: Float,
        actionState: Int,
        isCurrentlyActive: Boolean
    ) {
        super.onChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive)
        val icon = ContextCompat.getDrawable(recyclerView.context, R.drawable.ic_delete)!!
        val background = ContextCompat.getDrawable(recyclerView.context, R.color.error)!!
        val itemView= viewHolder.itemView
        val backgroundCornerOffset = 20
        val iconMargin = recyclerView.resources.getDimensionPixelSize(R.dimen.margin_24dp)
        val iconTop= itemView.top + (itemView.height - icon.intrinsicHeight) / 2
        val iconBottom = iconTop + icon.intrinsicHeight
        when {
            dX > 0 -> { // Swiping to the right
                val iconLeft = itemView.left + iconMargin
                val iconRight = iconLeft + icon.intrinsicWidth
                icon.setBounds(iconLeft, iconTop, iconRight, iconBottom)
                background.setBounds(
                    itemView.left, itemView.top,
                    itemView.left + dX.toInt() + backgroundCornerOffset, itemView.bottom
                )
            }
            dX < 0 -> { // Swiping to the left
                val iconLeft = itemView.right - iconMargin - icon.intrinsicWidth
                val iconRight = itemView.right - iconMargin
                icon.setBounds(iconLeft, iconTop, iconRight, iconBottom)
                background.setBounds(
                    itemView.right + dX.toInt() - backgroundCornerOffset,
                    itemView.top, itemView.right, itemView.bottom
                )
            }
            else -> { // view is unSwiped
                background.setBounds(0, 0, 0, 0)
            }
        }
        background.draw(c)
        icon.draw(c)
    }

    interface Callback {
        fun onSwipe(adapterPos: Int)
    }
}