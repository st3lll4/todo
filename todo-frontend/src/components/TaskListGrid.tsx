"use client";
import {useState, useEffect, useMemo} from "react";
import {TaskList} from "@/types/TaskList";
import {TaskListService} from "@/services/taskListService";
import PostItNote from "@/components/PostItNote";
import TaskListModal from "@/components/TaskListModal";
import {Button} from "flowbite-react";


export default function TaskListGrid() {
    const [taskLists, setTaskLists] = useState<TaskList[]>([]);
    const [selectedList, setSelectedList] = useState<TaskList | null>(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(true);

    const taskListService = useMemo(() => new TaskListService(), []);

    useEffect(() => {
        fetchTaskLists()
    }, []);


    const fetchTaskLists = async () => {
        try {
            setIsLoading(true);
            const response = await taskListService.getAllAsync();
            if (response.errors) {
                console.error(response.errors)
                return;
            }
            if (!response.data) return;
            console.log('data', response.data);
            setTaskLists(response.data);
        } catch (error) {
            console.error(error);
        } finally {
            setIsLoading(false);
        }
    };

    const openModal = (list?: TaskList) => {
        if (list) {
            setSelectedList(list);
        }
        setIsModalOpen(true);
    };

    const closeModal = async () => {
        setIsModalOpen(false);
        setSelectedList(null);

        setTimeout(async () => {
            await fetchTaskLists();
        }, 100);
    };



    const getPostItColor = (index: number) => {
        const colors = [
            "bg-pink-100 border-pink-200",
            "bg-blue-100 border-blue-200",
            "bg-green-100 border-green-200",
            "bg-pink-200 border-pink-300",
            "bg-purple-100 border-purple-200",
        ];
        return colors[index % colors.length];
    };

    if (isLoading) {
        return (
            <div className="flex justify-center items-center  h-64">
                <p className="text-gray-500">Loading your lists...</p>
            </div>
        );
    }

    return <div className="p-6">
        <div className="grid gap-6 grid-cols-5 justify-between items-center mb-6">
            <h1 className="text-2xl col-span-4 font-semibold text-green-900">My to-do lists</h1>

            <Button
                onClick={() => openModal()}
                className="col-span-1 px-6 py-3 bg-white hover:border-green-600 transition-colors hover:text-green-900 hover:bg-green-100 rounded-lg shadow-sm text-green-900 font-medium"
            >
                Create +
            </Button>

        </div>

        {taskLists.length === 0 && <div className="text-gray-500">
            Oh no! You don't have any lists yet. Create one to see something here
        </div>}
        <div className="grid max-w-[64rem] grid-cols-4 gap-6">

            {taskLists.map((list, index) => (
                <PostItNote
                    key={list.id}
                    list={list}
                    colorClass={getPostItColor(index)}
                    onClick={() => openModal(list)}
                />
            ))}
        </div>

        <TaskListModal
            list={selectedList}
            isOpen={isModalOpen}
            onClose={closeModal}
        />
    </div>


}

