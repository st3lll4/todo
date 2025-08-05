import {TaskList} from "@/types/TaskList";
import {Badge, Card, Checkbox} from "flowbite-react";
import {PriorityLevel} from "@/types/PriorityLevel";

interface PostItNoteProps {
    list: TaskList;
    colorClass: string;
    onClick: () => void;
}

export default function PostItNote({list, colorClass, onClick}: PostItNoteProps) {
    const visibleItems = list.listItems?.slice(0, 7);

    const remainingCount = (list.listItems?.length || 0) - 7;
    const completedCount = list.listItems?.filter(item => item.isDone).length || 0;
    const totalCount = list.listItems?.length || 0;

    console.log('list', list);

    return <Card
        onClick={onClick}
        className={`${colorClass} border-2 min-w-[12rem] cursor-pointer transform hover:scale-105 transition-transform duration-200 shadow-md hover:shadow-lg min-h-[16rem] relative`}
    >
        <div
            className="absolute -top-2 left-1/4 w-16 h-4 bg-yellow-200 opacity-50 rounded transform -rotate-12"></div>

        <div onClick={onClick} className="h-full flex flex-col">
            <h3 className="text-lg font-bold text-gray-800 mb-3 truncate">
                {list.title}
            </h3>

            <div className="flex-1">
                {visibleItems?.map((item, index) => (
                    <div
                        key={index}
                        className="flex items-center gap-2 my-1 text-sm"
                    >
                        <Checkbox
                            checked={item.isDone}
                            className="min-w-4 min-h-4"
                            readOnly
                        />

                        <span
                            className={`${
                                item.isDone
                                    ? 'line-through text-gray-500'
                                    : 'text-gray-700'
                            } truncate
                            ${
                                item.dueAt && new Date(item.dueAt) < new Date()
                                ? 'bg-red-400 border-red-900 rounded-sm px-1 text-red-900' : ''
                            }
                            `}
                        >
                                {item.description}
                            </span>
                        {item.priority == "Low" && <Badge color="green" className="text-xs">Low</Badge>}
                        {item.priority == "Medium" && <Badge color="warning" className="text-xs">Medium</Badge>}
                        {item.priority == "High" && <Badge color="pink" className="text-xs">High</Badge>}
                    </div>

                    ))}
                {remainingCount > 0 && (
                    <span className="text-green-500 text-sm font-light mt-3">
                            and {remainingCount} more
                        </span>
                )}
            </div>
            <hr className="my-2 text-gray-300" />
            <div className="flex justify-between items-center text-xs text-gray-600">
                <span>{completedCount}/{totalCount} done</span>
            </div>
        </div>
    </Card>

}
