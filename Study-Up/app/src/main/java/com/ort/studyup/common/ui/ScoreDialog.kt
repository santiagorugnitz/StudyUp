package com.ort.studyup.common.ui

import android.content.Context
import android.util.AttributeSet
import android.view.View
import android.widget.FrameLayout
import androidx.annotation.DrawableRes
import com.ort.studyup.R
import com.ort.studyup.common.getActivity
import kotlinx.android.synthetic.main.confirmation_dialog.view.*
import kotlin.math.roundToInt

class ScoreDialog(
    context: Context,
    private val score: Double,
    attrs: AttributeSet? = null
) : View(context, attrs) {


    fun show() {
        val view = inflate()
        view.dialogMsg.text = view.context.getString(R.string.exam_finished,((score * 10).roundToInt() / 10.0).toString())
        view.dialogButton.setOnClickListener { hide() }
        view.closeIcon.setOnClickListener { hide() }
    }


    private fun inflate(): View {
        val view = getActivity()?.findViewById<View>(R.id.dialogContainer)
        if (view != null) return view
        val root = getActivity()?.findViewById<FrameLayout>(android.R.id.content)
        return inflate(context, R.layout.score_dialog, root)
    }

    fun hide() {
        try {
            val root = getActivity()?.findViewById<FrameLayout>(android.R.id.content)
            val view = root?.findViewById<FrameLayout>(R.id.dialogContainer)
            root?.removeView(view)
        } catch (e: Exception) { }
    }

}