import { api } from '@/lib/axios';
import { useQuery } from 'react-query';
import { RecipeKeys } from './recipe.keys';
import queryString from 'query-string'
import { QueryParams, RecipeDto } from '../types';
import {PagedResponse, Pagination} from '@/types/api';
import {AxiosResponse} from 'axios';

const getRecipes = (queryString: string) => {
	queryString = queryString == '' 
		? queryString 
		: `?${queryString}`;

	return api.get(`/api/recipes?${queryString}`)
		.then((response: AxiosResponse<RecipeDto[]>) => {
			return {
				data: response.data as RecipeDto[],
				pagination: JSON.parse(response.headers['x-pagination']) as Pagination
			} as PagedResponse<RecipeDto>;
	});
};

export const useRecipes = ({ pageNumber, pageSize, filters, sortOrder }: QueryParams) => {
	let queryParams = queryString.stringify({ pageNumber, pageSize, filters, sortOrder });

	return useQuery(
		RecipeKeys.list(queryParams ?? ''),
		() => getRecipes(queryParams)
	);
};
