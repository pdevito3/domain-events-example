import { api } from '@/lib/axios';
import { AxiosError } from 'axios';
import { UseMutationOptions, useQueryClient, useMutation } from 'react-query';
import { IngredientKeys } from './ingredient.keys';
import { IngredientForUpdateDto } from '../types';

export const updateIngredient = (id: string, data: IngredientForUpdateDto) => {
	return api
		.put(`/api/ingredients/${id}`, data)
		.then(() => { });
};

export function useUpdateIngredient(id: string, options?: UseMutationOptions<void, AxiosError, IngredientForUpdateDto>) {
	const queryClient = useQueryClient()

	return useMutation(
		(updatedIngredient: IngredientForUpdateDto) => updateIngredient(id, updatedIngredient),
		{
			onSuccess: () => {
				queryClient.invalidateQueries(IngredientKeys.lists())
				queryClient.invalidateQueries(IngredientKeys.details())
			},
			...options
		});
}
