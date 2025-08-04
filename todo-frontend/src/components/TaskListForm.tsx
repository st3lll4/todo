"use client";
import {TaskListService} from "@/services/taskListService";
import {TaskList} from "@/types/TaskList";
import {
    Card,
    TextInput,
    HR,
    Checkbox,
    Datepicker,
    Select, ModalBody,
} from "flowbite-react";
import {PriorityLevel} from "@/types/PriorityLevel";
import {
    SubmitHandler,
    useForm,
    useFieldArray,
    Controller,
} from "react-hook-form";
import {useEffect, useMemo, useState} from "react";
import {ListItemService} from "@/services/listItemService";
import {ResultObject} from "@/types/ResultObject";

interface TaskListFormProps {
    taskListId?: string;
    onSuccess: (taskList: TaskList) => void;
    onError: (error: unknown) => void;
}

export default function TaskListForm({taskListId, onSuccess, onError}: TaskListFormProps) {
    const taskListService = useMemo(() => new TaskListService(), []);
    const listItemService = useMemo(() => new ListItemService(), [])
    const [list, setList] = useState<TaskList>();
    const [isLoading, setIsLoading] = useState(false);

    const {
        handleSubmit,
        control,
        reset,
        formState: {errors},
    } = useForm<TaskList>();

    const {fields, append, remove} = useFieldArray({
        control,
        name: "listItems",
    });

    useEffect(() => {
        fetchData()
    }, [taskListId, reset]);

    const fetchData = async () => {
        if (!taskListId) {
            return;
        }
        setIsLoading(true);
        try {
            const res = await taskListService.getAsync(taskListId);
            if (res.errors) {
                console.error(res.errors);
                onError(res.errors);
                return;
            }
            setList(res.data);
            if (res.data) {
                reset({
                    title: res.data.title,
                    listItems: res.data.listItems?.map((item) => ({
                        ...item,
                        dueAt: item.dueAt ? new Date(item.dueAt) : undefined
                    }))
                });
            }
        } finally {
            setIsLoading(false);
        }
    };

    const addTodo = () => {
        append({
            description: "",
            isDone: false,
            dueAt: undefined,
            priority: PriorityLevel.Low,
        });
    };

    const removeTodo = async (index: number) => {
        const res = await listItemService.deleteAsync(fields[index].id!);
        if (res.errors) {
            onError(res.errors);
            return;
        }
        remove(index);
    };

    const onSubmit: SubmitHandler<TaskList> = async (data) => {
        try {
            const taskListData: TaskList = {
                title: data.title,
            };

            let res: ResultObject<TaskList>;

            if (list?.id) {
                taskListData.id = list.id;
                res = await taskListService.updateAsync(taskListData);
                if (data.listItems) {
                    const existingItems = data.listItems.filter(item => item.id);
                    const newItems = data.listItems.filter(item => !item.id);

                    existingItems.map(async (item) => {
                        const result = await listItemService.updateAsync(item);
                        if (result.errors) {
                            onError(result.errors);
                        }
                        return result;
                    });

                    newItems.map(async (item) => {
                        const itemWithTaskListId = {...item, taskListId: list.id};
                        const result = await listItemService.addAsync(itemWithTaskListId);
                        if (result.errors) {
                            onError(result.errors);
                        }
                        return result;
                    });

                }

            } else {
                if (!data.listItems || data.listItems.length === 0) {
                    onError('Please add some todos to your list')
                    return;
                }
                res = await taskListService.addAsync(taskListData);
                if (res.data?.id) {
                    data.listItems.map(async (item) => {
                        const itemWithTaskListId = {...item, taskListId: res.data?.id};
                        const result = await listItemService.addAsync(itemWithTaskListId);
                        if (result.errors) {
                            onError(result.errors);
                        }
                        return result;
                    });
                }
            }

            if (res.errors) {
                onError(res.errors);
                return;
            }
            onSuccess(res.data!);
        } catch (error) {
            onError(error);
        }
    };

    if (isLoading) {
        return (
            <Card className="m-5 w-[48rem] bg-green-50">
                <div className="flex justify-center items-center h-32">
                    <p>Loading...</p>
                </div>
            </Card>
        );
    }

    const deleteList = (id: string) => {
        return async () => {
            try {
                const res = await taskListService.deleteAsync(id);
                if (res.errors) {
                    onError(res.errors);
                    return
                }
                onSuccess?.(res.data!);
            } catch (error) {
                onError(error);
            }
        }
    };
    return <ModalBody className="mx-4">
        <p className='mb-4'>Set a title<span className='text-red-600'>*</span></p>
        <Controller
            name="title"
            control={control}
            rules={{
                required: "Required",
                minLength: {value: 1, message: "Title cant be empty"},
            }}
            render={({field}) => (
                <TextInput
                    id="title"
                    type="text"
                    placeholder="Name your list"
                    value={field.value || ''}
                    onChange={field.onChange}
                    onBlur={field.onBlur}
                />
            )}
        />

        {errors.title && (
            <p className="text-red-500 text-sm mt-1">{errors.title.message}</p>
        )}

        <HR/>

        <div className="grid grid-cols-10 gap-2 mt-4 text-sm text-gray-700 items-center justify-items-start">
            <div className="col-span-1">Done</div>
            <div className="col-span-4">Description<span className='text-red-600'>*</span></div>
            <div className="col-span-3">Due at</div>
            <div className="col-span-1">Priority<span className='text-red-600'>*</span></div>
        </div>

        {fields.map((field, index) => (
            <div
                key={field.id}
                className="grid grid-cols-10 gap-2 py-2 items-center justify-items-stretch"
            >
                <Controller
                    name={`listItems.${index}.isDone`}
                    control={control}
                    render={({field: controllerField}) => (
                        <Checkbox
                            className="col-span-1 justify-self-center"
                            checked={controllerField.value}
                            onChange={controllerField.onChange}
                        />
                    )}
                />

                <Controller
                    name={`listItems.${index}.description`}
                    control={control}
                    rules={{
                        required: "Description is required",
                    }}
                    render={({field}) => (
                        <TextInput
                            placeholder="What do you need to do?"
                            className="col-span-4 w-full"
                            value={field.value || ''}
                            onChange={field.onChange}
                            onBlur={field.onBlur}
                        />
                    )}
                />


                <Controller
                    name={`listItems.${index}.dueAt`}
                    control={control}
                    render={({field: controllerField}) => (
                        <Datepicker
                            label="When is it due?"
                            className={`col-span-3 w-full ${isOverdue(field.dueAt) ? 'border-red-500' : ''}`}
                            value={controllerField.value ?? null}
                            onChange={controllerField.onChange}
                        />
                    )}
                />


                <Controller
                    name={`listItems.${index}.priority`}
                    control={control}
                    render={({field: controllerField}) => (
                        <Select
                            className="col-span-1 w-full"
                            value={controllerField.value}
                            onChange={controllerField.onChange}
                        >
                            <option value={PriorityLevel.Low}>Low</option>
                            <option value={PriorityLevel.Medium}>Medium</option>
                            <option value={PriorityLevel.High}>High</option>
                        </Select>
                    )}
                />

                {index >= 1 && (
                    <button
                        type="button"
                        onClick={() => removeTodo(index)}
                        className="col-span-1 text-sm justify-self-center text-red-500 hover:text-red-800"
                    >
                        x
                    </button>
                )}
            </div>
        ))}

        {errors.listItems && (
            <div className="mt-2">
                {fields.map((_, index) => (
                    <div key={index}>
                        {errors.listItems?.[index]?.description && (
                            <p className="text-red-500 text-sm">
                                Todo {index + 1}:{" "}
                                {errors.listItems[index]?.description?.message}
                            </p>
                        )}
                    </div>
                ))}
            </div>
        )}

        <div className="mt-4 flex justify-start">
            <button
                type="button"
                onClick={addTodo}
                className="flex items-center px-4 py-2 bg-green-400 text-green-800 rounded-full hover:bg-green-600 hover:text-green-200 transition-colors"
            >
                <p>+</p>
            </button>
        </div>


        <div className="mt-10 flex gap-4 justify-end">
            {list?.id &&
                <button
                    type="button"
                    className="px-6 py-2 bg-red-400 text-white rounded hover:bg-red-600 transition-colors"
                    onClick={deleteList(list.id)}
                >
                    Delete
                </button>}
            <button
                type="button"
                className="px-6 py-2 bg-green-800 text-white rounded hover:bg-green-600 transition-colors"
                onClick={handleSubmit(onSubmit)}
            >
                Save
            </button>
        </div>
    </ModalBody>
}