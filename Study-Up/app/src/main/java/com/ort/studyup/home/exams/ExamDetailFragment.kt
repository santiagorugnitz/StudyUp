package com.ort.studyup.home.exams

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.*
import com.ort.studyup.common.renderers.EmptyViewRenderer
import com.ort.studyup.common.renderers.ExamCardItemRenderer
import com.ort.studyup.common.renderers.ResultItemRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_exam_detail.*
import kotlinx.android.synthetic.main.fragment_exam_detail.swipeRefresh

class ExamDetailFragment : BaseFragment(), ExamCardItemRenderer.Callback {

    private val adapter = RendererAdapter()
    private val viewModel: ExamDetailViewModel by injectViewModel(ExamDetailViewModel::class)
    private var examId: Int = 0
    private var groupName = ""
    private var examName = ""

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_exam_detail, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        examId = arguments?.getInt(EXAM_ID_KEY) ?: 0
        groupName = arguments?.getString(GROUP_NAME_KEY) ?: ""
        examName = arguments?.getString(EXAM_NAME_KEY) ?: ""
        adapter.addRenderer(ExamCardItemRenderer(this))
        adapter.addRenderer(ResultItemRenderer())
        examCardList.layoutManager = LinearLayoutManager(requireContext())
        examCardList.adapter = adapter
        initViewModel()
        initUI()
        initRefresh()
    }

    private fun initUI() {
        title.text = examName
        if (groupName.isEmpty()) {
            addButton.setOnClickListener {
                findNavController().navigate(R.id.action_examDetailFragment_to_newExamCardFragment, Bundle().apply { putInt(EXAM_ID_KEY, examId) })
            }
            group.text = getString(R.string.unassigned)
        } else {
            addButton.visibility = View.GONE
            group.text = groupName
            adapter.setEmptyItem(EmptyViewRenderer.Item(getString(R.string.no_results_yet)),EmptyViewRenderer())
        }
    }

    private fun initRefresh() {
        if (groupName.isEmpty()) {
            swipeRefresh.isEnabled = false
        } else {
            swipeRefresh.isEnabled = true
            swipeRefresh.setOnRefreshListener {
                swipeRefresh.isRefreshing = false
                initViewModel()
            }
        }
    }

    private fun initViewModel() {
        if (groupName.isEmpty()) {
            viewModel.loadCards(examId).observe(viewLifecycleOwner, {
                adapter.setItems(it)
            })
        } else {
            viewModel.loadResults(examId).observe(viewLifecycleOwner, {
                adapter.setItems(it)
            })
        }
    }

    override fun onEditExamCard(id: Int, question: String, answer: Boolean) {
        findNavController().navigate(R.id.action_examDetailFragment_to_newExamCardFragment,
                Bundle().apply {
                    putInt(EXAM_ID_KEY, examId)
                    putInt(EXAM_CARD_ID_KEY, id)
                    putString(QUESTION_KEY, question)
                    putBoolean(ANSWER_KEY, answer)
                })
    }

}