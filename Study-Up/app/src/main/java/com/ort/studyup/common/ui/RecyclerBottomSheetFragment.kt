package com.ort.studyup.common.ui

import android.app.Dialog
import android.content.Context
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.content.res.ResourcesCompat
import androidx.recyclerview.widget.ItemTouchHelper
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.google.android.material.bottomsheet.BottomSheetBehavior
import com.google.android.material.bottomsheet.BottomSheetDialog
import com.google.android.material.bottomsheet.BottomSheetDialogFragment
import com.ort.studyup.R
import com.ort.studyup.common.models.Comment
import com.ort.studyup.common.renderers.CommentRenderer
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.recycler_bottom_sheet.view.*
import java.text.SimpleDateFormat

class RecyclerBottomSheetFragment : BottomSheetDialogFragment(), SwipeCallback.Callback {
    private var items = mutableListOf<Comment>()
    private val adapter = RendererAdapter()
    private var callback: Callback? = null

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.recycler_bottom_sheet, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        prepareList(view)
        loadData()
        view.closeIcon.setOnClickListener { dismiss() }
    }

    override fun onCreateDialog(savedInstanceState: Bundle?): Dialog {
        val dialog = super.onCreateDialog(savedInstanceState)
        dialog.setOnShowListener { dialogInterface ->
            val bottomSheetDialog = dialogInterface as BottomSheetDialog
            setupFullHeight(bottomSheetDialog)
        }
        return dialog
    }

    private fun setupFullHeight(bottomSheetDialog: BottomSheetDialog) {
        val bottomSheet = bottomSheetDialog.findViewById<View>(R.id.design_bottom_sheet) as View
        val behavior: BottomSheetBehavior<*> = BottomSheetBehavior.from(bottomSheet)
        val layoutParams = bottomSheet.layoutParams
        if (layoutParams != null) {
            layoutParams.height = (resources.displayMetrics.heightPixels * 0.6).toInt()
        }
        bottomSheet.layoutParams = layoutParams
        behavior.state = BottomSheetBehavior.STATE_EXPANDED
        bottomSheet.background = ResourcesCompat.getDrawable(resources, R.drawable.sheet_background, null)
    }


    private fun prepareList(view: View) {
        adapter.addRenderer(CommentRenderer())
        view.recyclerView.layoutManager = LinearLayoutManager(view.context)
        view.recyclerView.addItemDecoration(DividerHelper(requireContext(), view.resources.getDimensionPixelSize(R.dimen.margin_8dp)))
        view.recyclerView.adapter = adapter
        ItemTouchHelper(SwipeCallback(this)).attachToRecyclerView(view.recyclerView)
    }

    private fun loadData() {
        val mappedItems = items.map {
            CommentRenderer.Item(it.id, it.comment, it.username, SimpleDateFormat.getDateInstance().format(it.date))
        }
        adapter.setItems(mappedItems)
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)
    }

    override fun onDetach() {
        super.onDetach()
        callback = null
    }

    companion object {
        fun getInstance(): RecyclerBottomSheetFragment =
            RecyclerBottomSheetFragment()
    }

    fun setCallback(callback: Callback) {
        this.callback = callback
    }

    fun setItems(items: MutableList<Comment>) {
        this.items = items
    }


    interface Callback {
        fun onDelete(id: Int)
    }

    override fun onSwipe(adapterPos: Int) {
        items.removeAt(adapterPos)
        callback?.onDelete((adapter.getItems()[adapterPos] as CommentRenderer.Item).id)
        if (adapter.itemCount == 1) {
            dismiss()
        } else {
            adapter.removeItem(adapterPos)
        }

    }

}