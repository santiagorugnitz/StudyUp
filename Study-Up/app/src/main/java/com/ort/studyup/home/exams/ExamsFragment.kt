package com.ort.studyup.home.exams

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.Observer
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.EXAM_ID_KEY
import com.ort.studyup.common.renderers.ExamItemRenderer
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_exams.*

class ExamsFragment : BaseFragment(), ExamItemRenderer.Callback {

    private val adapter = RendererAdapter()
    private val viewModel: ExamsViewModel by injectViewModel(ExamsViewModel::class)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_exams, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
        initViewModel()
    }

    private fun initUI() {
        fab.setOnClickListener {
            findNavController().navigate(R.id.action_examsFragment_to_newExamFragment)
        }
        adapter.addRenderer(SubtitleRenderer())
        adapter.addRenderer(ExamItemRenderer(this))
        examList.layoutManager = LinearLayoutManager(requireContext())
        examList.adapter = adapter
    }

    private fun initViewModel() {
        viewModel.loadExams().observe(viewLifecycleOwner, Observer {
            adapter.setItems(it)
        })
    }

    override fun onExamClicked(examId: Int) {
        findNavController().navigate(R.id.action_examsFragment_to_examDetailFragment, Bundle().apply { putInt(EXAM_ID_KEY, examId) })
    }

    override fun onAssignExam(examId: Int, groupId: Int) {
        viewModel.onAssignExam(examId, groupId).observe(viewLifecycleOwner, Observer {
            initViewModel()
        })
    }

}