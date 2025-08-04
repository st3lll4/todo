import {TaskList} from "@/types/TaskList";
import {Modal, ModalHeader} from "flowbite-react";
import TaskListForm from "@/components/TaskListForm";

interface TaskListModalProps {
    list: TaskList | null;
    isOpen: boolean;
    onClose: () => void;
}

export default function TaskListModal({list, isOpen, onClose}: TaskListModalProps) {

    const handleSuccess = (taskList: TaskList) => {
        console.log("Success:", taskList);
        onClose();
    };

    const handleError = (error : unknown) => {
        alert("Error opening list: " + error);
        console.error("Error:", error);
    };

    return <Modal dismissible show={isOpen} onClose={onClose} size="4xl">
        <ModalHeader className='mx-4'>{ list ? `Editing "${list.title}"` : 'New'}</ModalHeader>

        {list && <TaskListForm
            taskListId={list.id}
            onError={handleError}
            onSuccess={handleSuccess}
        />}

        {!list && <TaskListForm
            onError={handleError}
            onSuccess={handleSuccess}
        />}

    </Modal>

}