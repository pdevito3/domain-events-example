import React from 'react';
import { toast, ToastContainer, ToastOptions } from 'react-toastify';

const Notifications = () => {
	return (
		<ToastContainer position={toast.POSITION.TOP_RIGHT} hideProgressBar={true} theme='colored' />
	);
};

Notifications.success = (message: string, options?: ToastOptions<{}>) => {
	toast.success(
		<div className='mx-2'>{message}</div>,
		Object.assign(
			{
				bodyClassName: 'py-3',
			},
			options
		)
	);
};

Notifications.error = (message: string, options?: ToastOptions<{}>) => {
	toast.error(
		<div className='mx-2'>{message}</div>,
		Object.assign(
			{
				bodyClassName: 'py-3',
			},
			options
		)
	);
};

export { Notifications };
